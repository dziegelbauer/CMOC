import {useFetchServices} from "../../hooks/ServiceHooks.ts";
import {useParams} from "react-router-dom";
import ApiStatus from "../ApiStatus.tsx";
import StatusCard from "./StatusCard.tsx";
import {useFetchCapability} from "../../hooks/CapabilityHooks.ts";
import {Service} from "../../types/Service.ts";

const ServiceStatus = () => {
    const { data, status, isSuccess } = useFetchServices();
    const { id} = useParams();
    let capabilityId;
    if (id) {
        capabilityId = parseInt(id);
    } else {
        capabilityId = 0;
    }
    
    const capabilityResponse = useFetchCapability(capabilityId);
    
    return (
        <div className="border p-3 mt-4">
            <div className="row">
                <div className="col-12">
                    {id && capabilityResponse.isSuccess && <h2>Services Supporting {capabilityResponse.data.name}</h2>}
                    {!id && <h2>All Services</h2>}  
                </div>
            </div>
            <hr/>
            <div className="row row-cols-1 row-cols-md-4 g-4">
                {!isSuccess && <ApiStatus status={status}/>}
                {isSuccess && data.map((s: Service) => {
                    return (
                        <StatusCard
                            status={s.status}
                            title={s.name}
                            id={s.id}
                            drillDownPath="/status/equipment/supporting"
                            key={s.id}
                        />
                    )
                })}
            </div>
        </div>
    )
}

export default ServiceStatus;