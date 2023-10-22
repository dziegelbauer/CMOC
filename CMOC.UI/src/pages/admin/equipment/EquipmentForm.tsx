import {Equipment} from "../../../types/Equipment.ts";
import {useEffect, useState} from "react";
import {useFetchServices} from "../../../hooks/ServiceHooks.ts";
import {useAddEquipmentType, useFetchEquipmentTypes} from "../../../hooks/EquipmentHooks.ts";
import {Link} from "react-router-dom";
import {useFetchLocations} from "../../../hooks/LocationHooks.ts";
import {EquipmentType} from "../../../types/EquipmentType.ts";

type Args = {
    equipment: Equipment;
    callback: (equipment: Equipment) => void;
}

type SelectionListRow = {
    text: string;
    id: number;
    checked: boolean;
}

const emptyEquipmentType: EquipmentType = {
    id: 0,
    name: ""
}

const EquipmentForm = ({equipment, callback}: Args) => {
    const [formEquipment, setFormEquipment] = useState(equipment);
    const servicesResponse = useFetchServices();
    const equipmentTypeResponse = useFetchEquipmentTypes();
    const locationResponse = useFetchLocations();
    const [supportedServices, setSupportedServices] = useState<SelectionListRow[]>([]);
    const addEquipmentTypeMutation = useAddEquipmentType();
    const [formEquipmentType, setFormEquipmentType] = useState(emptyEquipmentType);
    
    useEffect(() => {
        if(!servicesResponse.isSuccess) return;
        const serviceList = servicesResponse.data.map(s => {
            return {
                text: s.name,
                id: s.id,
                checked: formEquipment.supportedServices.includes(s.id)
            };
        });
        setSupportedServices(serviceList);
    }, [servicesResponse, formEquipment])
    
    return (
        <>
            <form onSubmit={(e) => {
                e.preventDefault();
                const equipmentToSubmit = {
                    ...formEquipment,
                    dependents: supportedServices
                        .filter(s => s.checked)
                        .map(s => s.id)
                };
                callback(equipmentToSubmit);
            }}>
            <input defaultValue={formEquipment.id} hidden/>
            <div className="border p-3 mt-4">
                <div className="row pb-2">
                    <div className="col-9">
                        <h2 className="text-primary pl-3">
                            {formEquipment.id == 0 ? "Create" : "Update"} Equipment
                        </h2>
                        <hr/>
                        <div className="mb-3 form-floating">
                            <select 
                                value={formEquipment.typeId} 
                                className="form-select"
                                id="equipmentTypeId"
                                onChange={(e) => {
                                    setFormEquipment({...formEquipment, typeId: parseInt(e.target.value)});
                                }}
                            >
                                <option disabled selected>Select Equipment Type</option>
                                {equipmentTypeResponse.isSuccess && equipmentTypeResponse.data.map(et => {
                                    return <option value={et.id} key={et.id}>{et.name}</option>;
                                })}
                            </select>
                            <label htmlFor="equipmentTypeId">Type</label>
                        </div>
                        <div className="mb-3 form-floating">
                            <input 
                                type="text"
                                id="equipmentSerialNumber"
                                value={formEquipment.serialNumber} 
                                className="form-control"
                                onChange={(e) => {
                                    setFormEquipment({...formEquipment, serialNumber: e.target.value});
                                }}
                            />
                            <label htmlFor="equipmentSerialNumber">Serial Number</label>
                        </div>
                        <div className="mb-3 form-floating">
                            <select 
                                value={formEquipment.locationId} 
                                className="form-select"
                                id="equipmentLocationId"
                                onChange={(e) => {
                                    setFormEquipment({...formEquipment, locationId: parseInt(e.target.value)});
                                }}
                            >
                                <option disabled selected>Select Location</option>
                                {locationResponse.isSuccess && locationResponse.data.map(et => {
                                    return <option value={et.id} key={et.id}>{et.name}</option>;
                                })}
                            </select>
                            <label htmlFor="equipmentLocationId">Location</label>
                        </div>
                        <div className="mb-3 form-floating">
                            <textarea
                                id="equipmentNotes"    
                                value={formEquipment.notes ?? ""} 
                                className="form-control" 
                                style={{"height": "8em"}}
                                onChange={(e) => {
                                    setFormEquipment({...formEquipment, notes: e.target.value})
                                }}
                            ></textarea>
                            <label htmlFor="equipmentNotes">Notes</label>
                        </div>
                        <div className="mb-3 form-floating">
                            <span>component mods here</span>
                        </div>
                        <div className="mb-3">
                            <span>Services Supported:</span>
                            <ul>
                                {supportedServices.map(s => {
                                    return (
                                        <li className="form-check form-switch" key={s.id}>
                                            <input type="hidden" value={s.id}/>
                                            <input className="form-check-input" 
                                                   type="checkbox" 
                                                   role="switch" 
                                                   id={`service_${s.id}`}
                                                   value={`${s.checked}`}
                                                   checked={s.checked}
                                                   onChange={() => {
                                                       const index = supportedServices.indexOf(s);
                                                       const newCapabilityList = Array.from(supportedServices);
                                                       newCapabilityList[index].checked = !supportedServices[index].checked;
                                                       setSupportedServices(newCapabilityList);
                                                   }}/>
                                            <label className="form-check-label" 
                                                   htmlFor={`service_${s.id}`}>{s.text}</label>
                                        </li>
                                    )
                                })}
                            </ul>
                        </div>
                    </div>
                    <div className="col-3">
                        <div className="row pb-2">
                            <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#newTypeDialog">
                                Add new equipment type
                            </button>
                        </div>
                    </div>
                </div>
                    {formEquipment.id === 0 &&
                        <button type="submit" className="btn btn-primary mx-1">Create</button>
                    }
                    else
                    {
                        <button type="submit" className="btn btn-primary mx-1">Update</button>
                    }
                    <Link to="/admin/equipment" className="btn btn-secondary">Back to List</Link>
                </div>
            </form>
            <div className="modal" tabIndex={-1} id="newTypeDialog">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <form id="newTypeForm" onSubmit={(e) => {
                            e.preventDefault();
                            addEquipmentTypeMutation.mutate(formEquipmentType);
                            setFormEquipmentType(emptyEquipmentType);
                        }}>
                            <div className="modal-header">
                                <h5 className="modal-title">New Equipment Type</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div className="modal-body">
                                <div className="mb-3 form-floating">
                                    <input 
                                        type="text" 
                                        id="typeName" 
                                        className="form-control"
                                        value={formEquipmentType.name}
                                        onChange={(e) => 
                                            setFormEquipmentType({...formEquipmentType, name: e.target.value})}
                                    />
                                    <label htmlFor="typeName">New Type Name</label>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                <button type="submit" className="btn btn-primary" data-bs-dismiss="modal" id="saveType">Save</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </>
    )
}

export default EquipmentForm;