using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EMS.DataAccess.Abstractions;

namespace EMS.DataAccess.EntityFrameworkCore
{
    public class ProvisioningStatus<TEntity> : IProvisioningStatus<TEntity>
    {
        public ProvisioningStatus(ProvisioningState state, IEnumerable<TEntity> entities, string message = "")
        {
            State = state;
            Entities = entities;
            Message = message;
        }

        public ProvisioningStatus(ProvisioningState state, TEntity entity, string message = "")
        {
            State = state;
            Entities = new[] {entity};
            Message = message;
        }

        [Key] public Guid RequestId { get; set; }
        public ProvisioningState State { get; set; }
        public string Message { get; set; }
        public IEnumerable<TEntity> Entities { get; set; }

        public override string ToString() => $"{RequestId} - {State.ToString()} ({Message})";
    }
}