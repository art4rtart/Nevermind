using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{    
    public enum PickableType
    {
        Weapon,
        Attachment,
        Ammunition,
        Equipment,
        Item
    }

    public enum PickableState
    {
        Ground,
        Bag,
        Pocket
    }

    public enum PickableMatrix
    {
        Square,
        Horizontal,
        Vertical
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }
}
