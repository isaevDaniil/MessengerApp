﻿using System;

namespace MessengerApp.Models
{
    public class ChatEventModel
    {
        public int Id { get; set; }

        public string CreatorName { get; set; }

        public DateTime EventTime { get; set; }

        public string EventType { get; set; }
    }
}
