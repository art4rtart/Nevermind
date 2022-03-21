using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<InventoryUIController>();
            return instance;
        }
    }
    private static InventoryUIController instance;

    [HideInInspector] public Transform pickedItemHolder;
    [HideInInspector] public Image viewPort;
    [HideInInspector] public Slot slot;
    [HideInInspector] public Element element;
    [HideInInspector] public bool isDropable;

    [HideInInspector] public Inventory inventory;

    private void Awake()
	{
        viewPort = GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetComponent<Image>();

        inventory = FindObjectOfType<Ground>();
    }

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.R))
		{
            element.RotateElement();
        }
	}

    public void CheckDropable()
	{
        isDropable = true;

        if(slot.index.y + element.Matrix().y > inventory.MapSize.y || slot.index.x + element.Matrix().x > inventory.MapSize.x)
        {
            isDropable = false;
            return;
		}

        for(int y = (int)slot.index.y; y < slot.index.y + element.Matrix().y; y++)
		{
            for (int x = (int)slot.index.x; x < slot.index.x + element.Matrix().x; x++)
            {
                if(inventory.Map[y, x] == true)
				{
                    isDropable = false;
                    break;
                }
            }
            if (!isDropable) break;
        }
    }
}
