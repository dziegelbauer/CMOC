import {TableColumnHeader} from "../../types/TableColumnHeader.ts";
import {useState} from "react";
import classes from "./Table.module.css";

type Args = {
    columnHeaders: TableColumnHeader[];
    handleSorting: (a:string, b:string) => void;
}

const TableHead = ({columnHeaders, handleSorting}: Args) => {
    const [sortField, setSortField] = useState("");
    const [sortOrder, setSortOrder] = useState("asc");
    
    const handleSortChange = (accessor: string) => {
        const newSortOrder =
            accessor === sortField && sortOrder === "asc" ? "desc" : "asc";
        setSortField(accessor);
        setSortOrder(newSortOrder);
        handleSorting(accessor, newSortOrder);
    }
    
    return (
        <thead>
            <tr>
                {columnHeaders.map(ch => {
                    const headerClass = ch.sortable
                        ? sortField === ch.accessor && sortOrder === "asc"
                            ? classes.up
                            : sortField === ch.accessor && sortOrder === "desc"
                                ? classes.down
                                : classes.default
                        : "";
                    return (
                        <th 
                            key={ch.accessor} 
                            onClick={() => {if(ch.sortable) handleSortChange(ch.accessor)}}
                            className={headerClass}
                        >
                            {ch.label}
                        </th>
                    );
                })}
            </tr>
        </thead>
    )
}

export default TableHead;