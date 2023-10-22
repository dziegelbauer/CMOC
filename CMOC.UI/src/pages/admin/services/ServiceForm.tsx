import {useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {Service} from "../../../types/Service.ts";
import {useFetchCapabilities} from "../../../hooks/CapabilityHooks.ts";

type Args = {
    service: Service;
    callback: (service: Service) => void;
}

type SelectionListRow = {
    text: string;
    id: number;
    checked: boolean;
}

const CapabilityForm = ({service, callback}: Args) => {
    const [formService, setFormCapability] = useState(service);
    const { data, isSuccess} = useFetchCapabilities();
    const [supportedCapabilities, setSupportedCapabilities] = useState<SelectionListRow[]>([]);
    
    useEffect(() => {
        if(!isSuccess) return;
        const capabilityList = data.map(c => {
            return {
                text: c.name, 
                id: c.id,
                checked: formService.dependents.includes(c.id) 
            }; 
        });
        setSupportedCapabilities(capabilityList);
    }, [data, formService])

    return (
        <form onSubmit={(e) => {
            e.preventDefault();
            const serviceToSubmit = {
                ...formService,
                dependents: supportedCapabilities
                    .filter(c => c.checked)
                    .map(c => c.id)
            };
            callback(serviceToSubmit);
        }}>
            <input defaultValue={service.id} hidden/>
            <div className="border p-3 mt-4">
                <div className="row pb-2">
                    <div className="col-9">
                        <h2 className="text-primary pl-3">
                            {service.id == 0 ? "Create" : "Update"} Service
                        </h2>
                        <hr/>

                        <div className="mb-3 form-floating">
                            <input
                                type="text"
                                id="serviceName"
                                value={formService.name}
                                className="form-control"
                                placeholder="Name"
                                onChange={(e) =>
                                    setFormCapability({...formService, name: e.target.value})}
                            />
                            <label htmlFor="serviceName" className="form-label">Name</label>
                        </div>
                        <div className="mb-3">
                            <span>Capabilities Supported:</span>
                            <ul>
                                {supportedCapabilities.map(c => {
                                    return (
                                        <li className="form-check form-switch" key={c.id}>
                                            <input type="hidden" value={c.id}/>
                                            <input className="form-check-input" 
                                                   type="checkbox" 
                                                   role="switch"
                                                   id={`capability_${c.id}`} 
                                                   value={`${c.checked}`} 
                                                   checked={c.checked} 
                                                   onChange={() => {
                                                       const index = supportedCapabilities.indexOf(c);
                                                       const newCapabilityList = Array.from(supportedCapabilities);
                                                       newCapabilityList[index].checked = !supportedCapabilities[index].checked;
                                                       setSupportedCapabilities(newCapabilityList);
                                                   }}/>
                                            <label className="form-check-label"
                                                   htmlFor={`capability_${c.id}`}>{c.text}</label>
                                        </li>
                                    )
                                })}
                            </ul>
                        </div>
                    </div>
                </div>
                {service.id === 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Create</button>
                }
                {service.id !== 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Update</button>
                }
                <Link to="/admin/services" className="btn btn-secondary">Back to List</Link>
            </div>
        </form>
    )
}

export default CapabilityForm;