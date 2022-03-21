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

	private void Awake()
	{
        viewPort = GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetComponent<Image>();
    }
}
