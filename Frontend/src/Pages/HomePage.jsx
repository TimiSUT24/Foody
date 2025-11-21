import {useState,useEffect} from 'react'
import {ProductService} from "../Services/ProductService"
import {useProductFilters} from "../Hooks/useProductFilters";
import ProductFilters from "../Components/ProductFilters";
import ProductCard from "../Components/ProductCard"
import "../CSS/HomePage.css"
import "../CSS/ProductCard.css"

export default function HomePage(){
    const [products, setProducts] = useState([]);

    
    const {
        filters,
        updateFilter,
    } = useProductFilters();

    useEffect (() => {
        ProductService.getProducts(filters).then(setProducts);
    }, [filters])

    //filter products by category
    const filteredProducts = products.filter(p => {
        if (filters.subSubCategoryId) {
            return p.subSubCategoryId === filters.subSubCategoryId;
        } else if (filters.subCategoryId) {
            return p.subCategoryId === filters.subCategoryId;
        } else if (filters.categoryId) {
            return p.categoryId === filters.categoryId;
        } else {
            return true; 
        }
    });


    return (
        <div className ="home-container">
            <ProductFilters
            filters={filters}
            updateFilter={updateFilter}>
            </ProductFilters>

            <div className ="product-grid">
                {filteredProducts.map(p => (                   
                    <ProductCard key={p.id} product={p}></ProductCard>         
                ))}
            </div>
        </div>
    )

}