using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot{
        public Item item;
        public Image image;

        public void Assign(Item assignedItem){
            item = assignedItem;
            if(item is Weapons){
                Weapons w = item as Weapons;
                image.enabled = true;
                image.sprite = w.weaponData.icon;
            }else{
                Passive p = item as Passive;
                image.enabled = true;
                image.sprite = p.data.icon;
            }
            Debug.Log(string.Format("Assigned {0} to player", item.name));
        }

        public void Clear(){
            item = null;
            image.enabled = false;
            image.sprite = null;
        }

        public bool IsEmpty(){
            return item == null? true : false;
        }
    }

    [System.Serializable]
    public class UpgradeUI{
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescritptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<Slot> weaponSlots = new List<Slot>(6);
    public List<Slot> passiveItemSlots = new List<Slot>(6);

    [Header("UI Elements")]
    public List<WeaponData> availableWeapons = new List<WeaponData>(); //Lista degli upgrade delle armi disponibili
    public List<PassiveData> availablePassiveItems = new List<PassiveData>();//Lista degli upgrade degli oggetti passivi disponibili
    public List<UpgradeUI> availableUIOptions = new List<UpgradeUI>();//lista dei upgrade disponibili

    PlayerStats player;

    void Start(){
        player = GetComponent<PlayerStats>();
    }

    //controlla se l'inventario ha oggetti di un certo tipo
    public bool HasType(ItemData type){
        return Get(type);
    }

    public Item Get(ItemData type){
        if(type is WeaponData){
            return Get(type as WeaponData);
        }else if(type is PassiveData){
            return Get(type as PassiveData);
        }else{
            return null;
        }
    }

    //trova un oggetto passivo di un determiato tipo
    public Passive Get(PassiveData type){
        foreach(Slot s in passiveItemSlots){
            Passive p = s.item as Passive;
            if(p.data == type){
                return p;
            }
        }
        return null;
    }

    //trova un arma di un determiato tipo
    public Weapons Get(WeaponData type){
        foreach(Slot s in weaponSlots){
            Weapons w = s.item as Weapons;
            if(w.weaponData == type){
                return w;
            }
        }
        return null;
    }

    //rimuovere un determinato oggetto
    public void Remove(ItemData data, bool removeUpgradeAvailability = false){
        if(data is Weapons){
            Remove(data, removeUpgradeAvailability);
        }else if(data is Passive){
            Remove(data, removeUpgradeAvailability);
        }else{
            return;
        }
    }

    //rimuovi un arma
    public void Remove(WeaponData data, bool removeUpgradeAvailability = false){
        //rimuovi quest'arma dalla pool degli Upgrade
        if(removeUpgradeAvailability){
            availableWeapons.Remove(data);
        }

        for(int i = 0; i<weaponSlots.Count; i++){
            Weapons w = weaponSlots[i].item as Weapons;
            if(w.weaponData == data){
                weaponSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return;
            }
        }
    }

    //rimuovi un oggetto passivo
    public void Remove(PassiveData data, bool removeUpgradeAvailability = false){
        //rimuovi quest'arma dalla pool degli Upgrade
        if(removeUpgradeAvailability){
            availablePassiveItems.Remove(data);
        }

        for(int i = 0; i< passiveItemSlots.Count; i++){
           Passive p = passiveItemSlots[i].item as Passive;
            if(p.data == data){
                passiveItemSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return;
            }
        }
    }

    //trova un slot vuoto e aggiungi un arma
    //ritorna l'indice dell'arma
    public int Add(WeaponData data){
        int slotNum = -1;

        //cerca di trovare lo slots vuoto
        for(int i = 0; i < weaponSlots.Capacity; i++){
            if(weaponSlots[i].IsEmpty()){
                slotNum = i;
                break;
            }
        }

        //se non ci sono slot liberi esci
        if(slotNum < 0){
            return -1;
        }

        //altrimenti crea l'arma nello slot
        //prendi il tipo di oggetto che vogliamo Spawnare
        Type weaponType = Type.GetType(data.behaviour);

        if(weaponType != null){
            //spawna il gameObject
            GameObject go = new GameObject(data.baseStats.name + "Controller");//genera il controller
            Weapons spawnedWeapon = (Weapons)go.AddComponent(weaponType);
            spawnedWeapon.Initialise(data);
            spawnedWeapon.transform.SetParent(transform);//setta l'arma come oggetto figlio dell'inventario
            spawnedWeapon.transform.localPosition = Vector2.zero;
            spawnedWeapon.OnEquip();

            //assegna l'arma allo slot
            weaponSlots[slotNum].Assign(spawnedWeapon);
            if(GameManager.instance != null && GameManager.instance.isLevelUp){
                GameManager.instance.EndLevelUp();
            }
            return slotNum;
        }else{
            Debug.LogWarning(string.Format("Invalid weapon type specified for {0}", data.name));
        }
        return -1;
    }

    //trova un slot vuoto e aggiungi un oggetto passivo
    //ritorna l'indice del oggetto passivo
    public int Add(PassiveData data){
        int slotNum = -1;

        //cerca di trovare lo slots vuoto
        for(int i = 0; i < passiveItemSlots.Capacity; i++){
            if(passiveItemSlots[i].IsEmpty()){
                slotNum = i;
                break;
            }
        }

        //se non ci sono slot liberi esci
        if(slotNum < 0){
            return slotNum;
        }

        //altrimenti crea l'arma nello slot
        //prendi il tipo di oggetto che vogliamo Spawnare
        //spawna il gameObject
        GameObject go = new GameObject(data.baseStats.name + "Controller");//genera il controller
        Passive spawnedPassiveItem = go.AddComponent<Passive>();
        spawnedPassiveItem.Initialise(data);
        spawnedPassiveItem.transform.SetParent(transform);//setta l'arma come oggetto figlio dell'inventario
        spawnedPassiveItem.transform.localPosition = Vector2.zero;

        //assegna l'oggetto passivo
        passiveItemSlots[slotNum].Assign(spawnedPassiveItem);

        if(GameManager.instance != null && GameManager.instance.isLevelUp){
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();
        return slotNum;
    }

    //se non sappiamo il tipo di arma che viene aggiunto, questa funzione lo determina
    public int Add(ItemData data){
        if(data is WeaponData){
            return Add(data as WeaponData);
        }else if(data is PassiveData){
            return Add(data as PassiveData);
        }else{
            return -1;
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex){
        if(weaponSlots.Count > slotIndex){
            Weapons weapon = weaponSlots[slotIndex].item as Weapons;

            //non fare il level up se sei già a livello massimo
            if(!weapon.DoLevelUp()){
                Debug.LogWarning(string.Format("Failed to level up {0}", weapon.name));
                return;
            }
        }

        if(GameManager.instance != null && GameManager.instance.isLevelUp){
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex){
        if(passiveItemSlots.Count > slotIndex){
            Passive passiveItem = passiveItemSlots[slotIndex].item as Passive;
            if(!passiveItem.DoLevelUp()){
                Debug.LogWarning(string.Format("Failed to level up {0}", passiveItem.name));
                return;
            }
        }

        if(GameManager.instance != null && GameManager.instance.isLevelUp){
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();
    }

    //determina quali upgradeOption dovrebbero apparire7
    void ApplyUpgradeOption(){

        //fai una copia delle armi e degli oggetti passivi disponibili
        //così da evitare problemi che possano intaccare le liste originali
        List<WeaponData> availableWeaponsUpgrade = new List<WeaponData>(availableWeapons);
        List<PassiveData> availablePassiveItemsUpgrade = new List<PassiveData>(availablePassiveItems);

        foreach(UpgradeUI upgradeOption in availableUIOptions){
            //se non ci sono più upgrade disponibili, allora uscima dalla funzione
            if(availableWeaponsUpgrade.Count == 0 && availablePassiveItemsUpgrade.Count == 0){
                return;
            }

            //determina se questo upgrade sia di un'arma o di un oggetto passivo
            int upgradeType;
            if(availableWeaponsUpgrade.Count == 0){
                upgradeType = 2;
            }else if(availablePassiveItemsUpgrade.Count == 0){
                upgradeType = 1;
            }else{
                upgradeType = UnityEngine.Random.Range(1,3);
            }

            //genere un upgrade attivo per un arma
            if(upgradeType == 1){
                WeaponData weapon = availableWeaponsUpgrade[UnityEngine.Random.Range(0, availableWeaponsUpgrade.Count)];
                //come prima cosa lo rimuovi dalla lista delle opzioni
                //così da non ritrovarci delle copie
                availableWeaponsUpgrade.Remove(weapon);
                if(weapon != null){
                    //attiva la UI
                    EnableUpgradeUI(upgradeOption);

                    //cerchiamo nelle armi disponibili l'arma di cui volgiamo abilitare il level up
                    //se la troviamo aggiungiamo un Event Listener al bottone per permettegli di fare il level up quando viene premuto
                    bool isLevelUp = false;
                    for(int i = 0; i < availableWeapons.Count; i++){
                        Weapons w = weaponSlots[i].item as Weapons;
                        if(w != null && w.weaponData == weapon){
                            //facciamo un controllo per vedere se l'arma è gia al massimo livello
                            if(weapon.maxLevel <= w.currentLevel){
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }

                            //setta il bottone 
                            upgradeOption.upgradeButton.onClick.AddListener(()=> LevelUpWeapon(i, i));
                            Weapons.Stats nextLevel = weapon.GetLevelData(w.currentLevel + 1);
                            upgradeOption.upgradeDescritptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = weapon.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    //se il codice arriva qui significa che stiamo aggiungendo una nuova arma
                    if(!isLevelUp){
                        upgradeOption.upgradeButton.onClick.AddListener(()=> Add(weapon));
                        upgradeOption.upgradeDescritptionDisplay.text = weapon.baseStats.description;
                        upgradeOption.upgradeNameDisplay.text = weapon.baseStats.name;
                        upgradeOption.upgradeIcon.sprite = weapon.icon;
                    }
                }
            }else if(upgradeType == 2){
                PassiveData passiveItem = availablePassiveItemsUpgrade[UnityEngine.Random.Range(0, availablePassiveItemsUpgrade.Count)];
                //come prima cosa lo rimuovi dalla lista delle opzioni
                //così da non ritrovarci delle copie
                availablePassiveItemsUpgrade.Remove(passiveItem);

                if(passiveItem != null){
                    //attiva la UI
                    EnableUpgradeUI(upgradeOption);

                    //cerchiamo nelle armi disponibili l'arma di cui volgiamo abilitare il level up
                    //se la troviamo aggiungiamo un Event Listener al bottone per permettegli di fare il level up quando viene premuto
                    bool isLevelUp = false;
                    for(int i = 0; i < availablePassiveItems.Count; i++){
                        Passive p = passiveItemSlots[i].item as Passive;
                        if(p != null && p.data == passiveItem){
                            //facciamo un controllo per vedere se l'arma è gia al massimo livello
                            if(passiveItem.maxLevel <= p.currentLevel){
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }

                            //setta il bottone 
                            upgradeOption.upgradeButton.onClick.AddListener(()=> LevelUpPassiveItem(i, i));
                            Passive.Modifier nextLevel = passiveItem.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescritptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = passiveItem.icon;
                            isLevelUp = true;
                            return;
                        }
                    }

                    //se il codice arriva qui significa che stiamo aggiungendo una nuova arma
                    if(!isLevelUp){
                        upgradeOption.upgradeButton.onClick.AddListener(()=> Add(passiveItem));
                        upgradeOption.upgradeDescritptionDisplay.text = passiveItem.baseStats.description;
                        upgradeOption.upgradeNameDisplay.text = passiveItem.baseStats.name;
                        upgradeOption.upgradeIcon.sprite = passiveItem.icon;
                    }
                }
            }
        }
    }

    void RemoveUpgradeOptions(){
        foreach(UpgradeUI upgradeOption in availableUIOptions){
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades(){
        RemoveUpgradeOptions();
        ApplyUpgradeOption();
    }

    void DisableUpgradeUI(UpgradeUI ui){
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui){
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    public List<Image> ReturnImage(List<Slot> list){
        List<Image> returnList = new List<Image>();
        foreach(Slot element in list){
            returnList.Add(element.image);
        }
        return returnList;
    }
}