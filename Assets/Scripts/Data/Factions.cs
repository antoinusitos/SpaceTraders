using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public enum Factions
    {
        NONE,
        CREW,
        TRAITOR,
        AI,
        PNJ
    }

    [System.Serializable]
    public enum FactionsRelations
    {
        ALLY,
        ENNEMIE,
        NEUTRAL
    }
}