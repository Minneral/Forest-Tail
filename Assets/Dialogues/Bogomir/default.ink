INCLUDE ../globals.ink
VAR npc = "Богомир"
VAR npc_portrait = "bogomir_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    {talking == false:
        Приветствую! Есть что на душе? #speaker:{npc} #portrait:{npc_portrait}
        ~ talking = true
    }
-> UnifiedChoices

=== UnifiedChoices ===
    {bogomir_quest_completed == false && bogomir_quest_assigned == false:
        * [Нужна ли вам помощь?]
            Слышал, у вас тут неспокойно. Может, помощь какая нужна? #speaker:{player_name} #portrait:player_default
            Точно! Тут гоблины шастают, покоя не дают. Поможешь от них избавиться? Благодарен буду! #quest:bogomir #speaker:{npc} #portrait:{npc_portrait}
            Добро, посмотрю, что можно сделать. Где их в последний раз видели? #speaker:{player_name} #portrait:player_default
            Видел их, как убегали из деревни через второй вход. На первом перекрестке налево свернули. Далеко они не ушли, это точно. #quest:bogomir #speaker:{npc} #portrait:{npc_portrait}
            Ладно, пойду посмотрю. #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    - else:
        {bogomir_quest_completed == true && bogomir_quest_finished == false:
            * Я разобрался с гоблинами. #speaker:{player_name} #portrait:player_default
                Да ну! Молодец! Вот тебе награда. #quest:bogomir #speaker:{npc} #portrait:{npc_portrait}
                -> UnifiedChoices
        }
    }

    + Расскажи про деревню. #speaker:{player_name} #portrait:player_default
        Эх, не о чем особо рассказывать. Живем здесь у черта на куличиках, зимы дожидаемся. Но лес у нас и вправду красивый! #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого! #speaker:{player_name} #portrait:player_default
        Удачи! Береги себя. #speaker:{npc} #portrait:{npc_portrait}
        -> END
}
