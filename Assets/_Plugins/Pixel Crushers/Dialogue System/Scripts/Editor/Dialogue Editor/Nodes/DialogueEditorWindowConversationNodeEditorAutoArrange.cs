using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.DialogueEditor
{

    /// <summary>
    /// This part of the Dialogue Editor window handles the auto-arrange
    /// feature for the conversation node editor.
    /// </summary>
    public partial class DialogueEditorWindow
    {

        private const float AutoWidthBetweenNodes = 20f;
        private const float AutoHeightBetweenNodes = 20f;

        private const float AutoStartX = 20f;
        private const float AutoStartY = 20f;

        private float canvasRectWidth { get { return canvasRectWidthMultiplier * DialogueEntry.CanvasRectWidth; } }
        private float canvasRectHeight { get { return DialogueEntry.CanvasRectHeight; } }

        [SerializeField]
        private int canvasRectWidthMultiplier = 1;

        private void CheckNodeWidths()
        {
            if (startEntry == null) return;
            if (!Mathf.Approximately(startEntry.canvasRect.width, canvasRectWidth))
            {
                foreach (var entry in currentConversation.dialogueEntries)
                {
                    var rect = entry.canvasRect;
                    var midX = rect.x + (rect.width / 2);
                    entry.canvasRect = new Rect(midX - (canvasRectWidth / 2), rect.y, canvasRectWidth, rect.height);
                }
            }
        }

        private void SetNodeWidthMultiplier(object data)
        {
            canvasRectWidthMultiplier = (int)data;
            CheckNodeWidths();
            ResetDialogueTreeSection();
            ResetConversationNodeSection();
        }

        private void CheckNodeArrangement()
        {
            if (startEntry == null) return;
            if ((startEntry.canvasRect.x == 0) && (startEntry.canvasRect.y == 0)) AutoArrangeNodes(!addNewNodesToRight);
        }

        private void ConfirmAndAutoArrangeNodes()
        {
            var result = EditorUtility.DisplayDialogComplex("Auto-Arrange Nodes",
                (multinodeSelection.nodes.Count > 1) ? "Are you sure you want to auto-arrange the selected nodes in this conversation?"
                : "Are you sure you want to auto-arrange the nodes in this conversation?", "Vertically", "Horizontally", "Cancel");
            switch (result)
            {
                case 0:
                    AutoArrangeNodes(true);
                    break;
                case 1:
                    AutoArrangeNodes(false);
                    break;
            }
        }

        private void AutoArrangeNodes(bool vertically)
        {
            InitializeDialogueTree();
            var tree = new List<List<DialogueEntry>>();
            ArrangeGatherChildren(dialogueTree, 0, tree);
            ArrangeTree(tree, vertically);
            ArrangeOrphans(vertically);
            SetDatabaseDirty("Auto-Arrange Nodes");
        }

        private void ArrangeGatherChildren(DialogueNode node, int level, List<List<DialogueEntry>> tree)
        {
            if (node == null) return;
            var skip = multinodeSelection.nodes.Count > 1 && !multinodeSelection.nodes.Contains(node.entry);
            while (tree.Count <= level)
            {
                tree.Add(new List<DialogueEntry>());
            }
            if (!(skip || tree[level].Contains(node.entry))) tree[level].Add(node.entry);
            if (node.hasFoldout)
            {
                for (int i = 0; i < node.children.Count; i++)
                {
                    var child = node.children[i];
                    ArrangeGatherChildren(child, level + 1, tree);
                }
            }
        }

        private float GetTreeWidth(List<List<DialogueEntry>> tree)
        {
            float maxWidth = 0;
            for (int i = 0; i < tree.Count; i++)
            {
                var level = tree[i];
                float levelWidth = level.Count * (canvasRectWidth + AutoWidthBetweenNodes);
                maxWidth = Mathf.Max(maxWidth, levelWidth);
            }
            return maxWidth;
        }

        private float GetTreeHeight(List<List<DialogueEntry>> tree)
        {
            float maxHeight = 0;
            for (int i = 0; i < tree.Count; i++)
            {
                var level = tree[i];
                float levelHeight = level.Count * (canvasRectHeight + AutoHeightBetweenNodes);
                maxHeight = Mathf.Max(maxHeight, levelHeight);
            }
            return maxHeight;
        }

        private void ArrangeTree(List<List<DialogueEntry>> tree, bool vertically)
        {
            if (vertically)
            {
                if (currentConversation == null || currentConversation.dialogueEntries == null || currentConversation.dialogueEntries.Count == 0) return;
                if (multinodeSelection != null && multinodeSelection.nodes.Count > 1)
                {
                    float treeWidth = GetTreeWidth(tree);
                    float x = AutoStartX;
                    if (orphans.Count > 0) x += canvasRectWidth + AutoWidthBetweenNodes;
                    float y = AutoStartY;
                    for (int level = 0; level < tree.Count; level++)
                    {
                        ArrangeLevel(tree[level], x, y, treeWidth, 0, vertically);
                        y += canvasRectHeight + AutoHeightBetweenNodes;
                    }
                }
                else
                {
                    // Using new algorithm provided by digiwombat [Fairmoon Museum]:
                    currentConversation.dialogueEntries[0].canvasRect = new Rect(2500, 1 * (canvasRectHeight + 24), canvasRectWidth, canvasRectHeight);
                    HandleChildren(currentConversation.dialogueEntries[0].outgoingLinks, currentConversation.dialogueEntries[0], 2);
                    MoveNodesToLeftSide();
                    nodeGridDict.Clear();
                    arrangedNodes.Clear();
                }
            }
            else
            {
                float treeHeight = GetTreeHeight(tree);
                float y = AutoStartY;
                if (orphans.Count > 0) y += canvasRectHeight + AutoHeightBetweenNodes;
                float x = AutoStartX;
                for (int level = 0; level < tree.Count; level++)
                {
                    ArrangeLevel(tree[level], x, y, 0, treeHeight, vertically);
                    x += canvasRectWidth + AutoWidthBetweenNodes;
                }
            }
        }

        private List<DialogueEntry> arrangedNodes = new List<DialogueEntry>();
        private Dictionary<int, DialogueEntry> nodeGridDict = new Dictionary<int, DialogueEntry>();
        private const int MaxNodeGridX = 1000;
        private int GetNodeGridKey(int x, int y) { return x + (y * MaxNodeGridX); }
        private int GetNodeGridKey(Vector2Int location) { return GetNodeGridKey(location.x, location.y); }

        private void HandleChildren(List<Link> links, DialogueEntry parent, int layer, int startX = 8, 
            List<Vector2Int> parentLocations = null)
        {
            Rect leftMost = new Rect();
            Rect rightMost = new Rect();
            int leftIndex = 0;
            int rightIndex = 0;

            for (int i = 0; i < links.Count; i++)
            {
                DialogueEntry child = currentConversation.dialogueEntries.Find(x => x.id == links[i].destinationDialogueID);
                if (child == null || arrangedNodes.Contains(child))
                {
                    continue;
                }

                while (nodeGridDict.ContainsKey(GetNodeGridKey(i + startX, layer)))
                {
                    startX++;
                }

                int xIndex = i + startX;

                nodeGridDict[GetNodeGridKey(xIndex, layer)] = child;
                arrangedNodes.Add(child);

                child.canvasRect = new Rect((xIndex * (canvasRectWidth + 10)), layer * (canvasRectHeight + 24), canvasRectWidth, canvasRectHeight);

                if (child.outgoingLinks.Count > 0)
                {
                    if (child.outgoingLinks.Count == 1)
                    {
                        if (parentLocations == null)
                        {
                            parentLocations = new List<Vector2Int>();
                        }
                        parentLocations.Add(new Vector2Int(xIndex, layer));
                        HandleChildren(child.outgoingLinks, child, layer + 1, xIndex, parentLocations);
                    }
                    else
                    {
                        int childIndexStart = xIndex - (child.outgoingLinks.Count / 2);

                        if (parentLocations != null)
                        {
                            //Fill gaps above single stacks with branches below
                            foreach (var location in parentLocations)
                            {
                                // Move parents in line with child, hypothetically
                                if (nodeGridDict.ContainsKey(GetNodeGridKey(location)))
                                {
                                    DialogueEntry value = nodeGridDict[GetNodeGridKey(location)];
                                    if (value != null)
                                    {
                                        value.canvasRect.x = (xIndex * (canvasRectWidth + 10));
                                    }
                                    nodeGridDict[GetNodeGridKey(xIndex, location.y)] = value;
                                    nodeGridDict.Remove(GetNodeGridKey(location));
                                }

                                for (int j = 0; j < child.outgoingLinks.Count; j++)
                                {
                                    nodeGridDict[GetNodeGridKey(childIndexStart + j, location.y)] = null;
                                }
                            }
                        }
                        HandleChildren(child.outgoingLinks, child, layer + 1, childIndexStart);
                    }
                }

                if (i == 0)
                {
                    leftMost = child.canvasRect;
                    leftIndex = xIndex;
                }
                rightMost = child.canvasRect;
                rightIndex = xIndex;

            }
            if (leftIndex != 0 && rightIndex != 0)
            {
                parent.canvasRect.x = ((leftIndex + rightIndex) * 0.5f) * (canvasRectWidth + 10);
            }
        }

        private void MoveNodesToLeftSide()
        {
            float minX = CanvasSize;
            foreach (var entry in currentConversation.dialogueEntries)
            {
                if (orphans.Find(x => x.entry == entry) != null) continue;
                minX = Mathf.Min(minX, entry.canvasRect.x);
            }
            if (minX > 10)
            {
                var dx = Mathf.Max(0, minX - (20 + ((orphans.Count > 0) ? canvasRectWidth : 0)));
                foreach (var entry in currentConversation.dialogueEntries)
                {
                    if (orphans.Find(x => x.entry == entry) != null) continue;
                    entry.canvasRect.x -= dx;
                }
            }
        }

        private void ArrangeLevel(List<DialogueEntry> nodes, float x, float y, float treeWidth, float treeHeight, bool vertically)
        {
            if (nodes == null || nodes.Count == 0) return;
            if (vertically)
            {
                float nodeCanvasWidth = treeWidth / nodes.Count;
                float nodeCanvasOffset = (nodeCanvasWidth - canvasRectWidth) / 2;
                for (int i = 0; i < nodes.Count; i++)
                {
                    float nodeX = x + (i * nodeCanvasWidth) + nodeCanvasOffset;
                    nodes[i].canvasRect = new Rect(nodeX, y, canvasRectWidth, canvasRectHeight);
                }
            }
            else
            {
                float nodeCanvasHeight = treeHeight / nodes.Count;
                float nodeCanvasOffset = (nodeCanvasHeight - canvasRectHeight) / 2;
                for (int i = 0; i < nodes.Count; i++)
                {
                    float nodeY = y + (i * nodeCanvasHeight) + nodeCanvasOffset;
                    nodes[i].canvasRect = new Rect(x, nodeY, canvasRectWidth, canvasRectHeight);
                }
            }
        }

        private void ArrangeOrphans(bool vertically)
        {
            if (vertically)
            {
                float y = AutoStartY;
                foreach (var orphan in orphans)
                {
                    var skip = multinodeSelection.nodes.Count > 1 && !multinodeSelection.nodes.Contains(orphan.entry);
                    if (skip) continue;
                    orphan.entry.canvasRect.x = AutoStartX;
                    orphan.entry.canvasRect.y = y;
                    y += orphan.entry.canvasRect.height + AutoHeightBetweenNodes;
                }
            }
            else
            {
                float x = AutoStartX;
                foreach (var orphan in orphans)
                {
                    var skip = multinodeSelection.nodes.Count > 1 && !multinodeSelection.nodes.Contains(orphan.entry);
                    if (skip) continue;
                    orphan.entry.canvasRect.x = x;
                    x += orphan.entry.canvasRect.width + AutoWidthBetweenNodes;
                    orphan.entry.canvasRect.y = AutoStartY;
                }
            }
        }

    }

}