import {useState,useEffect} from 'react'
import {ProductService} from "../Services/ProductService"
import {useProductFilters} from "../Hooks/useProductFilters";
import ProductFilters from "../Components/ProductFilters";

export default function HomePage(){
    const [products, setProducts] = useState([]);
    
    const {
        filters,
        updateFilter,
    } = useProductFilters();

    useEffect (() => {
        ProductService.getProducts(filters).then(setProducts);
    }, [filters])

    return (
        <div>
            <ProductFilters
            filters={filters}
            updateFilter={updateFilter}>
            </ProductFilters>

            <div className ="product-grid">
                {products.map(p => (
                  <div key={p.id} className="product-card">
                    <h4>{p.name}</h4>
                    <p>{p.brand}</p>
                    <p>{p.price}</p>
                  </div>  
                ))}
            </div>
        </div>
    )

}