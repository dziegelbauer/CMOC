import {useMutation, useQuery, useQueryClient} from "react-query";
import Problem from "../types/Problem.ts";
import {Equipment} from "../types/Equipment.ts";
import axios, {AxiosError} from "axios";
import {useNavigate} from "react-router-dom";
import {AxiosResponse} from "axios";
import {EquipmentType} from "../types/EquipmentType.ts";

const useFetchEquipmentItem = (id: number) => {
    return useQuery<Equipment, AxiosError<Problem>>(["equipment", id], () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Equipment/${id}`)
            .then(resp => resp.data.payload));
}

const useFetchEquipment = () => {
    return useQuery<Equipment[], AxiosError<Problem>>("equipment", () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Equipment`)
            .then(resp => resp.data.payload));
}

const useFetchEquipmentTypes = () => {
    return useQuery<EquipmentType[], AxiosError<Problem>>("equipmentTypes", () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/EquipmentType`)
            .then(resp => resp.data.payload));
}

const useAddEquipmentType = () => {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, EquipmentType>((et) =>
            axios.post(`${import.meta.env.VITE_BASE_API_URL}/api/v1/EquipmentType`, et), {
            onSuccess: () => {
                queryClient.invalidateQueries("equipmentTypes");
            }
        }
    );
}

const useAddEquipment = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Equipment>((c) =>
            axios.post(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Equipment`, c), {
            onSuccess: () => {
                queryClient.invalidateQueries("equipment");
                nav('/admin/equipment');
            }
        }
    );
}

const useUpdateEquipment = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Equipment>((c) =>
            axios.put(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Equipment`, c), {
            onSuccess: (_, __) => {
                queryClient.invalidateQueries("equipment");
                nav(`/admin/equipment`);
            }
        }
    );
}

const useDeleteEquipment = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, Equipment>((e) =>
            axios.delete(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Equipment/${e.id}`), {
            onSuccess: () => {
                queryClient.invalidateQueries("equipment");
                nav('/admin/equipment')
            }
        }
    );
}

export {
    useFetchEquipmentItem,
    useFetchEquipment,
    useFetchEquipmentTypes,
    useAddEquipment,
    useAddEquipmentType,
    useUpdateEquipment,
    useDeleteEquipment
};