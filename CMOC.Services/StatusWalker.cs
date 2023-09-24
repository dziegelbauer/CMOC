using CMOC.Domain;

namespace CMOC.Services;

public static class StatusWalker
{
    public static ObjectStatus ParseStatusGraph(this Capability capability)
    {
        var resultantStatus = ObjectStatus.FullyCapable;

        var nonRedundantServiceStatus = capability.SupportedBy
            .Where(csr => csr.RedundantWithId == null)
            .Select(csr => csr.Service)
            .Select(s => s.ParseStatusGraph())
            .ToList();

        if (nonRedundantServiceStatus.Any(s => s == ObjectStatus.NonCapable))
        {
            return ObjectStatus.NonCapable;
        }
        
        var redundancyIds = capability.SupportedBy
            .Where(csr => csr.RedundantWithId != null)
            .Select(csr => csr.RedundantWithId)
            .Distinct()
            .ToList();

        foreach (var redundancyId in redundancyIds)
        {
            var redundantServiceStatus = capability.SupportedBy
                .Where(csr => csr.RedundantWithId == redundancyId)
                .Select(csr => csr.Service)
                .Select(s => s.ParseStatusGraph())
                .ToList();

            if (redundantServiceStatus.All(s => s == ObjectStatus.NonCapable))
            {
                return ObjectStatus.NonCapable;
            }

            if (redundantServiceStatus.Any(s => s == ObjectStatus.NonCapable))
            {
                resultantStatus = ObjectStatus.PartiallyCapable;
            }
        }
        
        return resultantStatus;
    }

    public static ObjectStatus ParseStatusGraph(this Service service)
    {
        var resultantStatus = ObjectStatus.FullyCapable;

        var nonRedundantEquipmentStatus = service.SupportedBy
            .Where(ssr => ssr.RedundantWithId == null)
            .Select(ssr => ssr.Equipment)
            .Select(e => e.ParseStatusGraph())
            .ToList();
        
        if (nonRedundantEquipmentStatus.Any(s => s == ObjectStatus.NonCapable))
        {
            return ObjectStatus.NonCapable;
        }
        
        var redundancyIds = service.SupportedBy
            .Where(ssr => ssr.RedundantWithId != null)
            .Select(ssr => ssr.RedundantWithId)
            .Distinct()
            .ToList();

        foreach (var redundancyId in redundancyIds)
        {
            var redundantEquipmentStatus = service.SupportedBy
                .Where(ssr => ssr.RedundantWithId == redundancyId)
                .Select(ssr => ssr.Equipment)
                .Select(e => e.ParseStatusGraph())
                .ToList();

            if (redundantEquipmentStatus.All(s => s == ObjectStatus.NonCapable))
            {
                return ObjectStatus.NonCapable;
            }

            if (redundantEquipmentStatus.Any(s => s == ObjectStatus.NonCapable))
            {
                resultantStatus = ObjectStatus.PartiallyCapable;
            }
        }

        return resultantStatus;
    }

    public static ObjectStatus ParseStatusGraph(this Equipment equipment)
    {
        if (equipment.OperationalOverride is not null)
        {
            return equipment.OperationalOverride.Value
                ? ObjectStatus.FullyCapable
                : ObjectStatus.NonCapable;
        }

        if (equipment.Components.Any(cr =>
                cr.Components.Count(c => !c.Operational) >= cr.FailureThreshold))
        {
            return ObjectStatus.NonCapable;
        }

        return equipment.Components
            .SelectMany(cr => cr.Components.Select(c => c.Operational))
            .Any(componentOperational => componentOperational == false)
            ? ObjectStatus.PartiallyCapable
            : ObjectStatus.FullyCapable;
    }
}