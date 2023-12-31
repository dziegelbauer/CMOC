import axios, {AxiosError} from "axios";
import {useMutation, useQuery, useQueryClient} from "react-query";
import Problem from "../types/Problem.ts";
import {Service} from "../types/Service.ts";
import {useNavigate} from "react-router-dom";
import {AxiosResponse} from "axios";

const useFetchService = (id: number) => {
    return useQuery<Service, AxiosError<Problem>>(["services", id], () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}api/v1/Services/${id}`)
            .then(resp => resp.data.payload));
}

const useFetchServices = () => {
    return useQuery<Service[], AxiosError<Problem>>("services", () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}api/v1/Services`)
            .then(resp => resp.data.payload));
}

const useAddService = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Service>((s) =>
            axios.post(`${import.meta.env.VITE_BASE_API_URL}api/v1/Services`, s), {
            onSuccess: () => {
                queryClient.invalidateQueries("services");
                nav('/admin/services');
            }
        }
    );
}

const useUpdateService = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Service>((s) =>
            axios.put(`${import.meta.env.VITE_BASE_API_URL}api/v1/Services`, s), {
            onSuccess: (_, __) => {
                queryClient.invalidateQueries("services");
                nav(`/admin/services`);
            }
        }
    );
}

const useDeleteService = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, Service>((s) =>
            axios.delete(`${import.meta.env.VITE_BASE_API_URL}api/v1/Services/${s.id}`), {
            onSuccess: () => {
                queryClient.invalidateQueries("services");
                nav('/admin/services')
            }
        }
    );
}

export {
    useFetchService,
    useFetchServices,
    useAddService,
    useUpdateService,
    useDeleteService
};