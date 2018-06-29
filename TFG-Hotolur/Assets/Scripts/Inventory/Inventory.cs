using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script used to manage the inventory of the player
/// </summary>
public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Inventory");
            return;
        }
                
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;

    // Space of the inventory
    public int space = 20;

    private int index = 0;

    private TextMeshProUGUI weaponSelectedText;

    // List of the weapon kept in the inventory
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();

    private PlayerShoot playerShoot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetButtonDown("ChangeWeapon"))
        {
            ChangeWeapon();
        }

        // If there is some weapon in the inventory and weaponSelectedText isn't null then
        // show in the screen the weapon selected, if not then search for it
        if (weapons.Count > 0)
        {
            GameObject weaponSelectedTextGO = GameObject.FindGameObjectWithTag(Tags.weaponText);
            if (weaponSelectedTextGO != null)
            {
                weaponSelectedText = weaponSelectedTextGO.GetComponent<TextMeshProUGUI>();
                weaponSelectedText.text = string.Concat("Weapon selected: ", weapons[index].name);
            }
        }
    }

    // Called when an object is added to the inventory
    public bool Add (Item weapon)
    {
        // If there are no more space
        if(weapons.Count >= space)
        {
            // It writes a message to the console and do not add the weapon
            Debug.Log("Not enough room");
            return false;
        }

        bool pickup = true;
        for(int i = 0; i < weapons.Count; i++)
        {
            if(weapon.name == weapons[i].name)
            {
                pickup = false;
                break;
            }
        }
        if (pickup)
        {
            // Add the item to the list
            weapons.Add((Weapon)weapon);
        }

        ChangeWeapon();

        if(OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();

        return true;
    }

    // Called when the item in the inventory is used or removed
    public void Remove(Item weapon)
    {
        // Remove the item in the list
        weapons.Remove((Weapon)weapon);

        if (OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();
    }

    public void ChangeWeapon()
    {
        if(playerShoot == null)
            playerShoot = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<PlayerShoot>();
        int maxLength = weapons.Count;
        index = index + 1 < maxLength ? index + 1 : 0;
        playerShoot.EquipWeapon(weapons[index].maxDamage, weapons[index].range, weapons[index].fireRate, weapons[index].weaponType);
    }

    public void EquipActualWeapon()
    {
        if (playerShoot == null)
            playerShoot = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<PlayerShoot>();
        playerShoot.EquipWeapon(weapons[index].maxDamage, weapons[index].range, weapons[index].fireRate, weapons[index].weaponType);
    }

    // Getter of the list of weapons
    public List<Weapon> GetWeapons()
    {
        return weapons;
    }

    // Setter of the list of weapons
    public void SetItems(List<Weapon> weaponList)
    {
        weapons = weaponList;
    }

}
