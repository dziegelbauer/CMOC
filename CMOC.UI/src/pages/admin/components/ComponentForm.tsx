import {Component} from "../../../types/Component.ts";
import {useState} from "react";
import {useAddComponentType, useFetchComponentTypes} from "../../../hooks/ComponentHooks.ts";
import {useFetchEquipment} from "../../../hooks/EquipmentHooks.ts";
import {Link} from "react-router-dom";
import {ComponentType} from "../../../types/ComponentType.ts";

type Args = {
    component: Component;
    callback: (component: Component) => void;
}

const emptyComponentType: ComponentType = {
    id: 0,
    name: ""
}

const ComponentForm = ({component, callback}: Args) => {
    const [formComponent, setFormComponent] = useState(component);
    const componentTypeResponse = useFetchComponentTypes();
    const equipmentResponse = useFetchEquipment();
    const addComponentTypeMutation = useAddComponentType();
    const [formComponentType, setFormComponentType] = useState(emptyComponentType);
    
    return (
        <>
            <form onSubmit={(e) => {
                e.preventDefault();
                callback(formComponent);
            }}>
                <input defaultValue={formComponent.id} hidden/>
                <div className="border p-3 mt-4">
                    <div className="row pb-2">
                        <div className="col-9">
                            <h2 className="text-primary pl-3">
                                {formComponent.id == 0 ? "Create" : "Update"} Component
                            </h2>
                            <hr/>
                            <div className="mb-3 form-floating">
                                <select 
                                    value={formComponent.typeId}
                                    className="form-select"
                                    id="componentTypeId"
                                    onChange={(e) => {
                                        setFormComponent({...formComponent, typeId: parseInt(e.target.value)});
                                    }}
                                >
                                    <option disabled value={0}>Select Component Type</option>
                                    {componentTypeResponse.isSuccess && componentTypeResponse.data.map(ct => {
                                        return <option value={ct.id} key={ct.id}>{ct.name}</option>;
                                    })}
                                </select>
                                <label htmlFor="componentTypeId">Type</label>
                            </div>
                            <div className="mb-3 form-floating">
                                <input
                                    type="text"
                                    id="componentSerialNumber"
                                    value={formComponent.serialNumber}
                                    className="form-control"
                                    onChange={(e) => {
                                        setFormComponent({...formComponent, serialNumber: e.target.value});
                                    }}
                                />
                                <label htmlFor="componentSerialNumber">Serial Number</label>
                            </div>
                            <div className="mb-3 form-check form-switch">
                                <input 
                                    id="componentOperational"
                                    className="form-check-input" 
                                    type="checkbox" 
                                    role="switch" 
                                    value={`${formComponent.operational}`}
                                    checked={formComponent.operational}
                                    onChange={() => {
                                        setFormComponent({...formComponent, operational: !formComponent.operational});
                                    }}
                                />
                                <label className="form-check-label" htmlFor="componentOperational">Component Operational</label>
                            </div>
                            <div className="mb-3 form-floating">
                                <select 
                                    value={formComponent.equipmentId}
                                    className="form-select"
                                    id="componentEquipmentId"
                                    onChange={(e) => {
                                        setFormComponent({...formComponent, equipmentId: parseInt(e.target.value)});
                                    }}
                                >
                                    <option disabled value={0}>Select Equipment</option>
                                    {equipmentResponse.isSuccess && equipmentResponse.data.map(e => {
                                        const textRepresentation = `${e.typeName} sn: ${e.serialNumber}`
                                        return <option value={e.id} key={e.id}>{textRepresentation}</option>;
                                    })}
                                </select>
                                <label htmlFor="componentEquipmentId">Equipment</label>
                            </div>
                        </div>
                        <div className="col-3">
                            <div className="row pb-2">
                                <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#newTypeDialog">
                                    Add new component type
                                </button>
                            </div>
                        </div>
                    </div>
                    {formComponent.id === 0 &&
                        <button type="submit" className="btn btn-primary mx-1">Create</button>
                    }
                    {formComponent.id !== 0 &&
                        <button type="submit" className="btn btn-primary mx-1">Update</button>
                    }
                    <Link to="/admin/components" className="btn btn-secondary">Back to List</Link>
                </div>
            </form>
            <div className="modal" tabIndex={-1} id="newTypeDialog">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <form id="newTypeForm" onSubmit={(e) => {
                            e.preventDefault();
                            addComponentTypeMutation.mutate(formComponentType);
                            setFormComponentType(emptyComponentType);
                        }}>
                            <div className="modal-header">
                                <h5 className="modal-title">New Component Type</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div className="modal-body">
                                <div className="mb-3 form-floating">
                                    <input 
                                        type="text" 
                                        id="typeName" 
                                        className="form-control" 
                                        value={formComponentType.name}
                                        onChange={(e) => 
                                            setFormComponentType({...formComponentType, name: e.target.value})}
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

export default ComponentForm;