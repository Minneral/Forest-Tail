INCLUDE ../globals.ink
VAR npc = "Заслава"
VAR npc_portrait = "zaslava_default"

-> main

=== main ===

-> Greeting

=== Greeting ===
    Приветствую тебя, добрый путник! Как твои дела? #speaker:{npc} #portrait:{npc_portrait}
-> UnifiedChoices

=== UnifiedChoices ===
    {zaslava_quest_completed == false && zaslava_quest_assigned == false:
        * [Нужна ли тебе помощь по хозяйству?]
            Я странствую по этим местам и рад помогать людям. Может, тебе что-то нужно? #speaker:{player_name} #portrait:player_default
            Как же это приятно слышать! Знаешь, мне для отваров нужны грибы, а в кладовой совсем пусто. Поможешь собрать их в лесу? #quest:zaslava #speaker:{npc} #portrait:{npc_portrait}
            Конечно, помогу. Но где мне их искать? #speaker:{player_name} #portrait:player_default
            Иди через деревню в сторону леса. На развилке у пещеры поверни налево - там найдешь поляну, полную грибов. #speaker:{npc} #portrait:{npc_portrait}
            Сколько грибов тебе нужно? Полной корзины хватит? #speaker:{player_name} #portrait:player_default
            О, нет-нет, шести штук будет вполне достаточно. Не хочу слишком тебя нагружать! #speaker:{npc} #portrait:{npc_portrait}
            Шесть так шесть. Скоро вернусь. #speaker:{player_name} #portrait:player_default
            -> UnifiedChoices
    - else:
        {zaslava_quest_assigned == true && zaslava_quest_completed == false :
            * [Не подскажешь еще раз путь?]
                Конечно! Иди через деревню в сторону леса. Достигнув развилки у пещеры, поверни налево. Там и найдешь грибную поляну. #speaker:{npc} #portrait:{npc_portrait}
                Спасибо за подсказку! #speaker:{player_name} #portrait:player_default
                -> UnifiedChoices

        - else:
            {zaslava_quest_completed == true && zaslava_quest_finished == false:
                * [Я собрал грибы.]
                    Вот, как и просила, шесть грибов. Надеюсь, этого хватит. #speaker:{player_name} #portrait:player_default
                    Благодарю тебя, добрый человек! Теперь смогу приготовить необходимые отвары. Ты мне очень помог! #quest:zaslava #speaker:{npc} #portrait:{npc_portrait}
                    -> UnifiedChoices
            }
        }
    }

    + Расскажи про эти края. #speaker:{player_name} #portrait:player_default
        Здесь природа сказочная: густые леса, свежий воздух и люди дружелюбные. Только вот беда - разбойников последнее время слишком много появилось. Держись от них подальше. #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices

    * Чем занимаешься сегодня вечером? #speaker:{player_name} #portrait:player_default
        Весь вечер посвящу делам: убираюсь, отвары готовлю, да по дому хлопочу. #speaker:{npc} #portrait:{npc_portrait}
        Может, сделаешь перерыв и прогуляешься со мной по лесу? #speaker:{player_name} #portrait:player_default
        Было бы приятно, но, увы, времени совсем нет. Может, как-нибудь в другой раз? #speaker:{npc} #portrait:{npc_portrait}
        -> UnifiedChoices

    * До скорого! #speaker:{player_name} #portrait:player_default
        До встречи, будь осторожен в пути! #speaker:{npc} #portrait:{npc_portrait}
        -> END
