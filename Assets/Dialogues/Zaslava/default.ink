INCLUDE ../globals.ink
VAR npc = "Заслава"
VAR npc_portrait = "zaslava_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    {talking == false:
        Здрав будь, молодец #speaker:{npc} #portrait:{npc_portrait}
        ~ talking = true
    }
-> UnifiedChoices

=== UnifiedChoices ===
    {zaslava_quest_completed == false && zaslava_quest_assigned == false:
        * [Нужна ли тебе помощь по хозяйству?]
            Я не здешний, но прибыл издалека, чтобы людям помогать. Быть может тебе нужно помочь что-то сделать по хозяйству? #speaker:{player_name} #portrait:player_default
            Как интересно! Хмм... дай подумать. Мне для отваров ингредиенты нужны, а грибы как раз закончились. Сходишь за ними в лес? #quest:zaslava #speaker:{npc} #portrait:{npc_portrait}
            Конечно схожу. Только не подскажешь, где эти грибы растут. #speaker:{player_name} #portrait:player_default
            Иди в сторону леса до развилки с пещерой. На ней поверни налево, так ты придешь на поляну, где много грибов. #speaker:{npc} #portrait:{npc_portrait}
            Корзинки грибов тебе хватит? #speaker:{player_name} #portrait:player_default
            Ой, мне как-то неудобно тебя просить о таком. Мне и 6 штук будет достаточно #speaker:{npc} #portrait:{npc_portrait}
            Шесть так шесть #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    - else:
        {zaslava_quest_assigned == true && zaslava_quest_completed == false :
            * Не подскажешь еще раз путь? #speaker:{player_name} #portrait:player_default
                Конечно! Поляна с грибами находится за деревней. Иди в сторону леса к развилке с пещерой, на ней поверни налево и выйдешь на поляну. #speaker:{npc} #portrait:{npc_portrait}
                Спасибо #speaker:{player_name} #portrait:player_default
                -> UnifiedChoices
        
        - else:
            {zaslava_quest_completed == true && zaslava_quest_finished == false:
                * [Я собрал грибы] #speaker:{player_name} #portrait:player_default
                    Вот, грибов тебе собрал.
                    Спасибо тебе большое! #quest:zaslava #speaker:{npc} #portrait:{npc_portrait}
                    -> UnifiedChoices
            }
        }
    }

    + Расскажи про эти края #speaker:{player_name} #portrait:player_default
        Красивый край: чистый воздух, умиротворение. Одно лишь плохо, что разбойников последнее время много. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * Чем занимаешься сегодня вечером? #speaker:{player_name} #portrait:player_default
        Хах, извини, но я помолвлена, а потому заводить отношений не намерена #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого #speaker:{player_name} #portrait:player_default
        До встречи! #speaker:{npc} #portrait:{npc_portrait}
        -> END
