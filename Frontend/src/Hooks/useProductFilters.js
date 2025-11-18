import {useState} from 'react'

export function useProductFilters(){
    const [filters, setFilters] = useState({
        brand: "",
        categoryId: null,
        subCategoryId: null,
        subSubCategoryId: null,
        price: null
    });

    const updateFilter = (key, value) => {
        setFilters(prev => ({...prev, [key]: value}))
    };

    return{filters,updateFilter};

}