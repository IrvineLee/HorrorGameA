using System;

using Personal.Entity;
using Personal.Item;

[Serializable]
public class ItemEntity : GenericEntity
{
	public ItemType itemType;
	public string name;
}
