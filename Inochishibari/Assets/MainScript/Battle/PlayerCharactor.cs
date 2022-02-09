using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharactor", menuName = "CreatePlayer")]
public class PlayerCharactor : ScriptableObject
{
    //public RuntimeAnimatorController fieldAnim;
    public MoveCharaAnim.Sprites_Run Sprites_Run;
    public MoveCharaAnim.Sprites_Idle Sprites_Idle;
    public CardBase charaCard;

    
    public List<GetSkillLevel> getSkillLevel;

    


    [System.Serializable]
    public class GetSkillLevel
    {
        public int level;
        public CardBase skill;
    }

    
}
