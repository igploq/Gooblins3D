using UnityEngine;
using System.Collections.Generic; 

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform handTransform;

    private Camera playerCamera;

    private List<Transform> inventory = new List<Transform>(); 
    private List<Weapon> weaponScripts = new List<Weapon>();   
    private int currentSlot = -1; 
    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        HandleItemPickup();
        HandleItemDrop();
        HandleSlotSwitch();
    }

    void HandleItemPickup()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            bool isPickable = hit.transform.CompareTag("Pickable") || hit.transform.CompareTag("Weapon");

            if (isPickable && Input.GetKeyDown(KeyCode.E))
            {
                Weapon weapon = hit.transform.GetComponent<Weapon>();

             
                PickupItem(hit.transform, weapon);
            }
        }
    }

    void PickupItem(Transform item, Weapon weapon)
    {
        if (inventory.Count >= 4) return;

        item.SetParent(handTransform);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;

        item.GetComponent<Collider>().enabled = false;
        item.GetComponent<Rigidbody>().isKinematic = true;

        inventory.Add(item);
        weaponScripts.Add(weapon);

        item.gameObject.SetActive(false); 

        if (currentSlot == -1)
        {
            currentSlot = 0;
            ActivateSlot(currentSlot);
        }
    }

    void HandleItemDrop()
    {
        if (Input.GetKeyDown(KeyCode.G) && currentSlot != -1)
        {
            DropItem(currentSlot);
        }
    }

    void DropItem(int slot)
    {
        Transform item = inventory[slot];

        item.SetParent(null);

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(playerCamera.transform.forward * 5f, ForceMode.VelocityChange);
        }

        if (weaponScripts[slot] != null)
        {
            weaponScripts[slot].isEquipped = false;
            weaponScripts[slot].enabled = false;
        }

        inventory.RemoveAt(slot);
        weaponScripts.RemoveAt(slot);

        if (inventory.Count > 0)
        {
            currentSlot = Mathf.Clamp(slot, 0, inventory.Count - 1);
            ActivateSlot(currentSlot);
        }
        else
        {
            currentSlot = -1;
        }
    }

    void HandleSlotSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToSlot(3);
    }

    void SwitchToSlot(int slot)
    {
        if (slot >= inventory.Count) return;

        currentSlot = slot;
        ActivateSlot(currentSlot);
    }

    void ActivateSlot(int slot)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] != null)
            {
                inventory[i].gameObject.SetActive(i == slot);

                if (weaponScripts[i] != null)
                {
                    weaponScripts[i].isEquipped = (i == slot);
                    weaponScripts[i].enabled = (i == slot);
                }
            }
        }
    }
}