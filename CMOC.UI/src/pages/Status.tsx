import CapabilityStatus from "../components/status/CapabilityStatus.tsx";
import ServiceStatus from "../components/status/ServiceStatus.tsx";
import EquipmentStatus from "../components/status/EquipmentStatus.tsx";

type Args = {
    tier: "capability" | "service" | "equipment";
}

const Status = ({tier}: Args) => {
    switch(tier)
    {
        case "service":
            return <ServiceStatus/>;
        case "equipment":
            return <EquipmentStatus/>;
        default:
            return <CapabilityStatus/>;
    }
}

export default Status;