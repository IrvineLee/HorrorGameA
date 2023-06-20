using System;

using Personal.Entity;
using Personal.Item;

[Serializable]
public class LocalizationItemEntity : GenericEntity
{
	public ItemType itemType;
	public string description;
}
