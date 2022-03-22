using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
	#region singleton
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
	#endregion

	[SerializeField] public Transform pickedItemHolder;

    [Header("Drop")]
    [SerializeField] public Slot slot;
    [SerializeField] public Element element;
    [SerializeField] public bool isDropable;

    public Inventory Inventory { get { return inventory; } }
    Inventory inventory;

    public Image VeiwPort { get { return viewPort; } }
    Image viewPort;

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

        if(slot.Matrix.y + element.Matrix().y > inventory.InventorySize.y || slot.Matrix.x + element.Matrix().x > inventory.InventorySize.x)
        {
            isDropable = false;
            return;
		}

        for(int y = (int)slot.Matrix.y; y < slot.Matrix.y + element.Matrix().y; y++)
		{
            for (int x = (int)slot.Matrix.x; x < slot.Matrix.x + element.Matrix().x; x++)
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
