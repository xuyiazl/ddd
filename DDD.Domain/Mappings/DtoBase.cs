﻿using AutoMapper;
using DDD.Domain.Core;
using System;

namespace DDD.Domain.Common.Mappings
{
    public abstract class DtoBase<T> : IMapFrom<T>
    {
        public long Id { get; set; }
        public Status Status { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }

        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public abstract class DtoKeyBase<T> : IMapFrom<T>
    {
        public long Id { get; set; }

        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
