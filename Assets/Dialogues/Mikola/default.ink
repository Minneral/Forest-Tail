INCLUDE ../globals.ink
VAR npc = "Микола"
VAR npc_portrait = "mikola_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    {talking == false:
        Здравствуй #speaker:{npc} #portrait:{npc_portrait}
        ~ talking = true
    }
-> UnifiedChoices

=== UnifiedChoices ===
    {mikolaMemories_quest_completed == false && mikolaMemories_quest_assigned == false:
        * [Помощь нужна?]
            Нужно ли тебе помочь в каком деле? #speaker:{player_name} #portrait:player_default
            Я со всем сам потихоньку справляюсь. Если так хочешь помочь - можешь заставить Якова работать, а то он только и делает, что в карты играет. #speaker:{npc} #portrait:{npc_portrait} #quest:mikolaMemories
            Вот как, посмотрю что можно с этим сделать. #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    - else:
        {mikolaMemories_quest_assigned == true && mikolaMemories_quest_completed == false :
            * Нужно лишь заставить его работать? #speaker:{player_name} #portrait:player_default
                Да, а то совсем уже обленел, только и делает, что играет #speaker:{npc} #portrait:{npc_portrait}
                -> UnifiedChoices
        
        - else:
            {mikolaMemories_quest_completed == true && mikolaMemories_quest_finished == false:
                * Я заставил его работать
                    Вот как, спасибо тебе! #quest:mikolaMemories #speaker:{npc} #portrait:{npc_portrait}
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
