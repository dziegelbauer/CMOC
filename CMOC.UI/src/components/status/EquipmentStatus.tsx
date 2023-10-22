import {useFetchService} from "../../hooks/ServiceHooks.ts";
import {useParams} from "react-router-dom";
import ApiStatus from "../ApiStatus.tsx";
import StatusCard from "./StatusCard.tsx";
import {useFetchEquipment} from "../../hooks/EquipmentHooks.ts";
import {Equipment} from "../../types/Equipment.ts";

const EquipmentStatus = () => {
    const { data, status, isSuccess } = useFetchEquipment();
    const { id} = useParams();
    let serviceId;
    if (id) {
        serviceId = parseInt(id);
    } else {
        serviceId = 0;
    }

    const serviceResponse = useFetchService(serviceId);

    return (
        <div className="border p-3 mt-4">   
            <div className="row">
                <div className="col-12">
                    {id && serviceResponse.isSuccess && <h2>Equipment Supporting {serviceResponse.data.name}</h2>}
                    {!id && <h2>All Equipment</h2>}
                </div>
            </div>
            <hr/>
            <div className="row row-cols-1 row-cols-md-4 g-4">
                {!isSuccess && <ApiStatus status={status}/>}
                {isSuccess && data.map((e: Equipment) => {
                    return (
                        <StatusCard
                            status={e.status}
                            title={`${e.typeName} sn: ${e.serialNumber}`}
                            id={e.id}
                            drillDownPath="#"
                            key={e.id}
                        />
                    )
                })}
            </div>
        </div>
    )
}

export default EquipmentStatus;