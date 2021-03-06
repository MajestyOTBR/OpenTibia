﻿using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public static class Constants
    {
        public static readonly string OnlyProtocol86Allowed = "Only protocol 8.6 allowed.";

        public static readonly string AccountNameOrPasswordIsNotCorrect = "Account name or password is not correct.";

        public static readonly string ThereIsNoWay = "There is no way.";

        public static readonly string SorryNotPossible = "Sorry, not possible.";

        public static readonly string YouCanNotUseThisItem = "You cannot use this item.";
        
        public static readonly string YouCanNotMoveThisObject = "You cannot move this object.";

        public static readonly string YouCanNotTakeThisObject = "You cannot take this object.";

        public static readonly string YouCanNotThrowThere = "You cannot throw there.";

        public static readonly string YouCanNotUseThere = "You cannot use there.";

        public static readonly string ThisIsImpossible = "This is impossible.";

        public static readonly string ThereIsNotEnoughtSpace = "There is not enought space.";

        public static readonly string YouMayNotAttackThisCreature = "You may not attack this creature.";

        public static readonly string TargetLost = "Target lost.";

        public static string PlayerActionSchedulerEvent(Player player)
        {
            return "Player_Action_" + player.Id;
        }

        public static readonly int PlayerSchedulerEventDelay = 200;

        public static readonly int PlayerWaitFollowEventDelay = 1 * 1000;

        public static string PlayerAttackSchedulerEvent(Player player)
        {
            return "Player_Attack_" + player.Id;
        }

        public static readonly int PlayerAttackEventDelay = 2 * 1000;
        
        public static readonly string GlobalLightSchedulerEvent = "Global_Light";

        public static readonly int GlobalLightSchedulerEventInterval = 10 * 1000;
        
        public static readonly string GlobalCreaturesSchedulerEvent = "Global_Creatures";

        public static readonly int GlobalCreaturesSchedulerEventInterval = 1 * 1000;
    }
}