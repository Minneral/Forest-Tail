INCLUDE ../globals.ink
VAR npc = "Всеслав"
VAR npc_portrait = "vseslav_default"

-> main

=== main ===

-> Greeting

=== Greeting ===
    Здрав будь, боярин! Чем ты меня порадуешь сегодня? #speaker:{npc} #portrait:{npc_portrait}
-> UnifiedChoices

=== UnifiedChoices ===
    {vseslav_quest_completed == false && vseslav_quest_assigned == false:
        {vseslav_quest_canstart == false:
        * [Могу ли я как-нибудь тебе помочь, Волхв?]
            Я странствую и помогаю людям. Быть может, тебе что-нибудь нужно, старче? #speaker:{player_name} #portrait:player_default
            Если что и нужно, то это задание не для слабонервных. Сначала узнай у других, нужна ли им помощь. А если и после этого будешь готов - возвращайся. #speaker:{npc} #portrait:{npc_portrait}
            ->UnifiedChoices
        - else:
        * [Я помог всем жителям]
            Со всеми делами в деревне я разобрался. Так что за работа такая у тебя? #speaker:{player_name} #portrait:player_default
            Ну, раз все выполнил и готов на большее, слушай. В пещере неподалеку завелся тролль. Он угрожает деревне. Разберись с ним, и я тебя наградой не обижу. #quest:vseslav #speaker:{npc} #portrait:{npc_portrait}
            Где находится эта пещера? #speaker:{player_name} #portrait:player_default
            Иди через второй выход из деревни, до второй развилки. На развилке сверни направо, и выйдешь к глубокой пещере. Там и обитает это чудище. #speaker:{npc} #portrait:{npc_portrait}
            Что ж, посмотрим, какой он на деле. #speaker:{player_name} #portrait:player_default
            Удачи тебе, храбрец. #speaker:{npc} #portrait:{npc_portrait}
        ->UnifiedChoices
        }
        
    - else:
        {vseslav_quest_assigned == true && vseslav_quest_completed == false :
            * [Не подскажешь еще раз дорогу к пещере?]
                Конечно. Иди через второй выход из деревни, до второй развилки. На ней поверни направо, и выйдешь к пещере. #speaker:{npc} #portrait:{npc_portrait}
                -> UnifiedChoices

        - else:
            {vseslav_quest_completed == true && vseslav_quest_finished == false:
                * [Я уничтожил тролля]
                    Разбил я тролля вашего, в пещере обитающего. Больше никого не потревожит. #speaker:{player_name} #portrait:player_default
                    Спасибо тебе, ратник! Вот тебе заслуженная награда. #quest:vseslav #speaker:{npc} #portrait:{npc_portrait}
                    За награду, конечно, спасибо. А больше некому помочь в деревне? #speaker:{player_name} #portrait:player_default
                    У нас ты уже всем помог. Но в соседних деревнях, говорят, тоже беды хватает. Сходи к ним, если силы будут. #speaker:{npc} #portrait:{npc_portrait}
                    Понял. До скорого, Всеслав. #speaker:{player_name} #portrait:player_default
                    -> END
            }
        }
    }

    + Тяжело ли Волхвское ремесло? #speaker:{player_name} #portrait:player_default
        Тяжело, да по-своему приятно. Людям помогать — дело непростое, а знания старинные беречь ещё сложнее. Но, как говорится, кто, если не мы? #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices

    * Правду говорят, что вы, волхвы, с духами говорите? #speaker:{player_name} #portrait:player_default
        Правду. Только не каждый дух к разговору склонен. Бывают такие, что и слова не скажут, а только угрозами веют. Но с некоторыми дружба выходит крепкая. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices

    * До скорого! #speaker:{player_name} #portrait:player_default
        До встречи. Будь осторожен в пути и помни: путь богатыря нелегок, но славен. #speaker:{npc} #portrait:{npc_portrait}
        -> END
