INCLUDE ../globals.ink
VAR npc = "Яков"
VAR npc_portrait = "yakov_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    {talking == false:
        Привет #speaker:{npc} #portrait:{npc_portrait}
        ~ talking = true
    }
-> UnifiedChoices

=== UnifiedChoices ===
    {yakov_quest_completed == false && yakov_quest_assigned == false:
        * [Не отлынивай от работы]
            Микола говорит, что заставить тебя работать невозможно, только и делаешь, что в карты играешь #speaker:{player_name} #portrait:player_default
            Я пытаюсь лишь занять себя чем-то, а в картах я чувствую отдушину. Я бы даже сказал, что я в них преуспел. #speaker:{npc} #portrait:{npc_portrait}
            Преуспел, не преуспел, но работать все равно нужно! #speaker:{player_name} #portrait:player_default
            Давай так, если выиграешь меня в карты, то я так уж и быть, пойду работать. #quest:yakov #speaker:{npc} #portrait:{npc_portrait}
            Закасай рукава, сейчас я покажу тебе как нужно играть #speaker:{player_name} #portrait:player_default #puzzle:memories
            {yakov_quest_completed == true:
                    Вот досада, ладно, твоя взяла. Пойду поработаю. #quest:yakov #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Я же говорил тебе, что я лучший в этой игре #speaker:{npc} #portrait:{npc_portrait}
            }
            -> UnifiedChoices
    - else:
        {yakov_quest_assigned == true && yakov_quest_completed == false :
            * Хочешь отыграться? #speaker:{player_name} #portrait:player_default
                Давай, в прошлый раз тебе повезло #speaker:{npc} #portrait:{npc_portrait} #puzzle:memories
                {yakov_quest_completed == true:
                    Вот досада, ладно, твоя взяла. Пойду поработаю. #quest:yakov #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Хаха, повезет в другой раз! #speaker:{npc} #portrait:{npc_portrait}
            }
                -> UnifiedChoices
        
        - else:
            {yakov_quest_completed == true && yakov_quest_finished == false:
                * [Я собрал грибы]
                    Вот, грибов тебе собрал. #speaker:{player_name} #portrait:player_default
                    Спасибо тебе большое! #quest:zaslava #speaker:{npc} #portrait:{npc_portrait}
                    -> UnifiedChoices
            }
        }
    }

    + Как досуг проводишь? #speaker:{player_name} #portrait:player_default
        Смотрю в окно или играю в карты. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого #speaker:{player_name} #portrait:player_default
        До встречи! #speaker:{npc} #portrait:{npc_portrait}
        -> END
