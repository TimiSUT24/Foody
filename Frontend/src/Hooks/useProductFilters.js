import {useState} from 'react'

export function useProductFilters(){
    const [filters, setFilters] = useState({
        name: "",
        brand: "",
        categoryId: null,
        subCategoryId: null,
        subSubCategoryId: null,
        price: 0
    });

    const updateFilter = (newValues) => {
    setFilters(prev => {
        let changed = false;

        for (const key in newValues) {
            if (prev[key] !== newValues[key]) {
                changed = true;
                break;
            }
        }

        // 🔑 no state update → no re-render → no loop
        if (!changed) return prev;

        return { ...prev, ...newValues };
    });
};
    return{filters,updateFilter};

}