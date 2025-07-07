﻿using MassTransit;

namespace AuthAPI.Sagas.Instances
{
    public class UserState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = "";
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
