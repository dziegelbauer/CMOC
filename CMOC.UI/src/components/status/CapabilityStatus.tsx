import {useFetchCapabilities} from "../../hooks/CapabilityHooks.ts";
import StatusCard from "./StatusCard.tsx";
import ApiStatus from "../ApiStatus.tsx";
import {Capability} from "../../types/Capability.ts";

const CapabilityStatus = () => {
    const { data, status, isSuccess } = useFetchCapabilities();
    
    return (
        <div className="border p-3 mt-4">
            <div className="row">
                <div className="col-12">
                    <h2>All Capabilities</h2>
                </div>
            </div>
            <hr/>
            <div className="row row-cols-1 row-cols-md-4 g-4">
                {!isSuccess && <ApiStatus status={status}/>}
                {isSuccess && data.map((c: Capability) => {
                    return (
                        <StatusCard 
                            status={c.status} 
                            title={c.name} 
                            id={c.id} 
                            drillDownPath="/status/services/supporting" 
                            key={c.id}
                        />
                    )
                })}
            </div>
        </div>
    )
}

export default CapabilityStatus;