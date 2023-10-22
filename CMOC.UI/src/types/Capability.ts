import {Service} from "./Service.ts";

export type Capability = {
    id: number;
    name: string;
    dependencies: Service[];
    status: number;
}