INCLUDE ../globals.ink
EXTERNAL foo()
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
    {mikolaMemories_quest_completed == false && mikolaMemories_quest_assigned == true && mikolaMemories_player_losed == false:
        * [Не отлынивай от работы]
            Микола говорит, что заставить тебя работать невозможно, только и делаешь, что в карты играешь #speaker:{player_name} #portrait:player_default
            Я пытаюсь лишь занять себя чем-то, а в картах я чувствую отдушину. Я бы даже сказал, что я в них преуспел. #speaker:{npc} #portrait:{npc_portrait}
            Преуспел, не преуспел, но работать все равно нужно! #speaker:{player_name} #portrait:player_default
            Давай так, если выиграешь меня в карты, то я так уж и быть, пойду работать. #quest:yakov #speaker:{npc} #portrait:{npc_portrait}
            Закасай рукава, сейчас я покажу тебе как нужно играть #speaker:{player_name} #portrait:player_default #puzzle:memories
            {mikolaMemories_quest_completed == true:
                    Вот досада, ладно, твоя взяла. Пойду поработаю. #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Я же говорил тебе, что я лучший в этой игре #speaker:{npc} #portrait:{npc_portrait}
            }
            -> UnifiedChoices
    - else:
        {mikolaMemories_quest_assigned == true && mikolaMemories_quest_completed == false :
            + [Хочу отыграться]
                Давай еще раз сыграем, в прошлый раз тебе повезло #speaker:{player_name} #portrait:player_default
                Как хочешь, давай снова сыграем. #puzzle:memories #speaker:{npc} #portrait:{npc_portrait}
                {mikolaMemories_quest_completed == true:
                    Вот досада, ладно, твоя взяла. Пойду поработаю. #quest:mikolaMemories #speaker:{npc} #portrait:{npc_portrait}
                - else:
                    Хаха, повезет в другой раз! #speaker:{npc} #portrait:{npc_portrait}
                }
            -> UnifiedChoices
        }
    }

    + Как досуг проводишь? #speaker:{player_name} #portrait:player_default
        Смотрю в окно или играю в карты. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого #speaker:{player_name} #portrait:player_default
        Пока #speaker:{npc} #portrait:{npc_portrait}
        -> END
}
