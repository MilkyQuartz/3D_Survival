    using UnityEngine;

    public enum TrickItemType
    {
        JumpPad,
        MovePad,
        Ladder,
        DivingJumpPad,
        Ray
    }

    [CreateAssetMenu(fileName = "TrickItem", menuName = "Trick New Item")]
    public class TrickItemData : ScriptableObject
    {
        [Header("Info")]
        public string displayName;
        public string description;
        public TrickItemType trickItemType;

        [Header("JumpPad")]
        public GameObject jumpPadPrefab;
        public float jumpForce;

        [Header("MovePad")]
        public GameObject movePadPrefab;
        public float moveSpeed;
        public float moveDistance;

        [Header("Ladder")]
        public GameObject radderPrefab;
        public float radderSpeed;

        [Header("DivingJumpPad")]
        public GameObject DivingjumpPadPrefab;
        public float DivingjumpForce;

        [Header("Ray")]
        public GameObject rayPrefab;
}
