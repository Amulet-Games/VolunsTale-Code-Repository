using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MenuSingletonsStack : MonoBehaviour
    {
        [Header("Hud / Windows (Drops).")]
        public MainHudManager mainHudManager;
        public CharacterRegisterWindow charRegistWindow;

        [Header("Selection Menus (Drops).")]
        public SelectionMenuManager selectionMenuManager;
        public EquipmentMenuManager equipmentMenuManager;
        public InstructionMenuManager instructionMenuManager;

        [Header("Checkpoint Menus (Drops).")]
        public CheckpointMenuManager checkpointMenuManager;

        public static MenuSingletonsStack singleton;
        void Awake()
        {
            if (singleton != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                singleton = this;
            }
        }
    }
}