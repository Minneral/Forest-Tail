INCLUDE ../globals.ink
VAR npc = "Микола"
VAR npc_portrait = "mikola_default"
VAR talking = false

-> main

=== main ===

-> Greeting

=== Greeting ===
    Добрый день, путник! Чем могу помочь? #speaker:{npc} #portrait:{npc_portrait}
-> UnifiedChoices

=== UnifiedChoices ===
    {mikolaMemories_quest_completed == false && mikolaMemories_quest_assigned == false:
        * [Помощь нужна?]
            Может, нужна помощь с чем-нибудь? #speaker:{player_name} #portrait:player_default
            Спасибо за предложение, но пока справляюсь сам. Хотя, если хочешь помочь, уговори Якова взяться за дело - он только и делает, что карты раскладывает. #speaker:{npc} #portrait:{npc_portrait} #quest:mikolaMemories
            Ясно. Попробую поговорить с ним. #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    - else:
        {mikolaMemories_quest_assigned == true && mikolaMemories_quest_completed == false :
            * [Только заставить его работать?]
                Да, иначе никак. Совсем обленился, от работы бегает как чёрт от ладана. #speaker:{npc} #portrait:{npc_portrait}
                -> UnifiedChoices

        - else:
            {mikolaMemories_quest_completed == true && mikolaMemories_quest_finished == false:
                * [Я заставил его работать.]
                    Ты молодец! Спасибо за помощь, иначе я бы сам с этим не справился. #quest:mikolaMemories #speaker:{npc} #portrait:{npc_portrait}
                    -> UnifiedChoices
            }
        }
    }

    + Тяжело работать на мельнице? #speaker:{player_name} #portrait:player_default
        Нет, не тяжело, но ответственность большая. Ошибёшься с пропорциями - и всё на смарку. А в остальном работа даже спокойная. Главное - втянуться. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices
    * До скорого! #speaker:{player_name} #portrait:player_default
        До встречи, путник! Береги себя. #speaker:{npc} #portrait:{npc_portrait}
        -> END
