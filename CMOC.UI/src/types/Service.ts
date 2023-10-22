import {Equipment} from "./Equipment.ts";

export type Service = {
    id: number;
    name: string;
    dependencies: Equipment[];
    dependents: number[];
    status: number;
}