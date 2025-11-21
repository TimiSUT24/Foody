import {useState} from 'react'

export function useProductFilters(){
    const [filters, setFilters] = useState({
        name: "",
        brand: "",
        categoryId: null,
        subCategoryId: null,
        subSubCategoryId: null,
        price: null
    });

    const updateFilter = (newValues) => {
    setFilters(prev => ({ ...prev, ...newValues }));
};

    return{filters,updateFilter};

}