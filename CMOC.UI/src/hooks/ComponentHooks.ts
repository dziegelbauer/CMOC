import axios, {AxiosError, AxiosResponse} from "axios";
import {useMutation, useQuery, useQueryClient} from "react-query";
import Problem from "../types/Problem.ts";
import {useNavigate} from "react-router-dom";
import {Component} from "../types/Component.ts";
import {ComponentType} from "../types/ComponentType.ts";

const useFetchComponent = (id: number) => {
    return useQuery<Component, AxiosError<Problem>>(["components", id], () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Components/${id}`)
            .then(resp => resp.data.payload));
}

const useFetchComponents = () => {
    return useQuery<Component[], AxiosError<Problem>>("components", () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Components`)
            .then(resp => resp.data.payload));
}

const useFetchComponentTypes = () => {
    return useQuery<ComponentType[], AxiosError<Problem>>("componentTypes", () =>
        axios
            .get(`${import.meta.env.VITE_BASE_API_URL}/api/v1/ComponentType`)
            .then(resp => resp.data.payload));
}

const useAddComponent = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Component>((c) =>
            axios.post(`${import.meta.env.VITE_BASE_API_URL}/api/v1/Components`, c), {
            onSuccess: () => {
                queryClient.invalidateQueries("components");
                nav('/admin/components');
            }
        }
    );
}

const useAddComponentType = () => {
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, ComponentType>((ct) =>
        axios.post(`${import.meta.env.VITE_BASE_API_URL}api/v1/ComponentType`, ct), {
        onSuccess: () => {
            queryClient.invalidateQueries("componentTypes");
        }
        }
    );
}

const useUpdateComponent = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Component>((c) =>
            axios.put(`${import.meta.env.VITE_BASE_API_URL}api/v1/Components`, c), {
            onSuccess: (_, __) => {
                queryClient.invalidateQueries("components");
                nav(`/admin/components`);
            }
        }
    );
}

const useDeleteComponent = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, Component>((c) =>
            axios.delete(`${import.meta.env.VITE_BASE_API_URL}api/v1/Components/${c.id}`), {
            onSuccess: () => {
                queryClient.invalidateQueries("components");
                nav('/admin/component')
            }
        }
    );
}

export {
    useFetchComponent,
    useFetchComponents,
    useFetchComponentTypes,
    useAddComponent,
    useAddComponentType,
    useUpdateComponent,
    useDeleteComponent
};