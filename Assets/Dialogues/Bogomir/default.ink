INCLUDE ../globals.ink
VAR npc = "Богомир"
VAR npc_portrait = "bogomir_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    {talking == false:
        Тебе есть что спросить? #speaker:{npc} #portrait:{npc_portrait}
        ~ talking = true
    }
-> UnifiedChoices

=== UnifiedChoices ===
    {bogomir_quest_completed == false && bogomir_quest_assigned == false:
        * [Нужна ли вам помощь?]
            Слыхал неспокойное у вас тут место, быть может нужно помочь вам? #speaker:{player_name} #portrait:player_default
            Да, тут ты прав! Недавно гоблины в округе объявились, да жить не дают. Помоги избавиться от них, а я уж в долге не останусь. #quest:bogomir #speaker:{npc} #portrait:{npc_portrait}
            -> UnifiedChoices
    - else:
        {bogomir_quest_completed == true && bogomir_quest_finished == false:
            * Я разобрался с гоблинами #speaker:{player_name} #portrait:player_default
                Вот как, отличная работа! Вот тебе награда. #quest:bogomir #speaker:{npc} #portrait:{npc_portrait}
                -> UnifiedChoices
        }
    }

    + Расскажи про деревню #speaker:{player_name} #portrait:player_default
        Ох, было бы чего рассказывать. Сидим у черта на куличиках, да живем от зимы к зиме. Единственное что, наверное, лес у нас красивый. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого #speaker:{player_name} #portrait:player_default
        -> END
