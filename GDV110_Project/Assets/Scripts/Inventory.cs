/***
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) [2024] Media Design School
File Name : Inventory.cs
Description : Contains All The Implementation Code For The Inventory... NOT THE DECK
Author : Joe Rickwood
Mail : Joe.Rickwood@mds.ac.nz
**/

using System.Collections.Generic;

#region Manager
[System.Serializable]
public class InventoryManager
{
    protected List<Upgrade> upgrades;

    //Adds Item To The Inventory Then Activates It
    public void AddItem(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
        upgrade.Activate(this);
    }

    //Returns True If Successfully Removed
    public bool RemoveItem(int index) 
    {
        if(upgrades.Count <= index)
        {
            return false;
        }

        upgrades.RemoveAt(index);
        return true;
    }

    //Checks If Item With The Same ID Exists In The Inventory
    public bool HasItem(int ID)
    {
        for (int i = 0; i < upgrades.Count; i++) 
        { 
            if(upgrades[i].ID == ID)
            {
                return true;
            }
        }

        return false;
    }

    //Returns A Item From A Specific Index From The Inventory
    //Returns Null If Index Exceeds Inventory Length
    public Upgrade GetItem(int index)
    {
        return upgrades.Count <= index ? upgrades[index] : null;
    }
}
#endregion

#region Upgrade
//Upgrade Item Template
[System.Serializable]
public class Upgrade
{
    public string name = "";
    public int ID = 0;
    public string description = "";
    public int price = 0;

    public Upgrade(string name, int ID, string description, int price)
    {
        this.name = name;
        this.ID = ID;
        this.description = description;
        this.price = price;
    }

    //Initializes The Upgrade With Text In Name And Descriptions
    //Should Be Called When Added To An Item Table
    public virtual void Init()
    {
        name = "DEFAULT";
        ID = -1;
        description = "Nothing Here";
        price = 0;
    }

    //Should Be Cloned When Added To A Waffle Or Character
    //This Is So Variables Are Not Changed GLOBALLY, Instead Each Instance Of Said Upgrade Are Added
    //Dont Clone When Adding To Topping Cards, Only When Actually ADDED To The Waffle
    public virtual Upgrade Clone()
    {
        return (Upgrade)this.MemberwiseClone();
    }

    public virtual void Activate(/* HostEntity Add In Here */ InventoryManager manager)
    {
        //Link Trigger Events To Methods Invoked My Inventory Manager
        //E.G onItemAddEvent += OnItemAdd
    }

}

#endregion

//--------------------------------------------
//DEFINE ALL ITEM DEFINITION CLASSES DOWN HERE

#region ExampleItem
//EG.

/*
 
    public class Item1 : Upgrade
    {
        public override void Init() {
            name = "Item1";
            ID = 0;
            description = "This Is A Item";
            price = 1;
        }

        public override void Activate(InventoryManager manager)
        {
            //Do Stuff
        }
    }


*/
#endregion