INCLUDE ../globals.ink
EXTERNAL foo()
VAR npc = "Яков"
VAR npc_portrait = "yakov_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    Эй, привет! Как делишки? #speaker:{npc} #portrait:{npc_portrait}
-> UnifiedChoices

=== UnifiedChoices ===
    {mikolaMemories_quest_completed == false && mikolaMemories_quest_assigned == true && mikolaMemories_player_losed == false:
        * [Не отлынивай от работы]
            Микола говорит, что заставить тебя работать невозможно, только и делаешь, что в карты играешь. #speaker:{player_name} #portrait:player_default
            Ох, да я просто пытаюсь найти себе занятие. В картах я ощущаю себя на коне. Честно говоря, я даже стал мастером! #speaker:{npc} #portrait:{npc_portrait}
            Преуспел, не преуспел, но работать все равно нужно! Хватит отлынивать! #speaker:{player_name} #portrait:player_default
            Ладно, давай так: если ты победишь меня в картах, я отправлюсь работать. #quest:yakov #speaker:{npc} #portrait:{npc_portrait}
            Ну держись, сейчас покажу тебе, как настоящие профи играют! #speaker:{player_name} #portrait:player_default #puzzle:memories
            {mikolaMemories_quest_completed == true:
                    Черт, ну ладно, твоя взяла. Пойду поработаю. #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Я же говорил, что я лучший в этой игре! #speaker:{npc} #portrait:{npc_portrait}
            }
            -> UnifiedChoices
    - else:
        {mikolaMemories_quest_assigned == true && mikolaMemories_quest_completed == false :
            + [Хочу отыграться]
                Давай еще раз сыграем, в прошлый раз тебе повезло. #speaker:{player_name} #portrait:player_default
                О, как хочешь. Вперед, давай попробуем еще раз! #puzzle:memories #speaker:{npc} #portrait:{npc_portrait}
                {mikolaMemories_quest_completed == true:
                    Вот досада, ну ладно. Пойду поработаю. #quest:mikolaMemories #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Хаха, на этот раз удача на моей стороне! #speaker:{npc} #portrait:{npc_portrait}
                }
            -> UnifiedChoices
        }
    }
    {mikolaMemories_quest_completed == true && mikolaMemories_quest_finished == true: 
        + [Давай сыграем в карты]
            О, как хочешь. Вперед, давай попробуем еще раз! #puzzle:memories #speaker:{npc} #portrait:{npc_portrait}
            Хорошей была партия #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    }

    + Как проводишь время? #speaker:{player_name} #portrait:player_default
        Обычно смотрю в окно или играю в карты. А ты чем занят? #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого! #speaker:{player_name} #portrait:player_default
        Пока-пока! Удачи! #speaker:{npc} #portrait:{npc_portrait}
        -> END
}
