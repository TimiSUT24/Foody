import {useState,useEffect} from 'react'
import {ProductService} from "../Services/ProductService"
import {useProductFilters} from "../Hooks/useProductFilters";
import ProductFilters from "../Components/ProductFilters";
import ProductCard from "../Components/ProductCard"
import { useSearchParams } from 'react-router-dom';
import "../CSS/HomePage.css"
import "../CSS/ProductCard.css"

export default function HomePage(){
    const [products, setProducts] = useState([]);
    const [searchParams] = useSearchParams();
    
    const {
        filters,
        updateFilter,
    } = useProductFilters();

    const categoryIdParam = searchParams.get("categoryId");
    const subCategoryIdParam = searchParams.get("subCategoryId");
    const subSubCategoryIdParam = searchParams.get("subSubCategoryId");

    useEffect(() => {
    const controller = new AbortController();//cancel async operation thats in progress

    ProductService
        .getProducts(filters, controller.signal)
        .then(setProducts)
        .catch(err => {
            if (err.name !== "AbortError") {
                console.error(err);
            }
        });

    return () => controller.abort();
}, [filters]);

useEffect(() => {
    if (!categoryIdParam) return;

    updateFilter({ categoryId: categoryIdParam ?Number(categoryIdParam) : null,
        subCategoryId: subCategoryIdParam ? Number(subCategoryIdParam) : null,
        subSubCategoryId: subSubCategoryIdParam ? Number(subSubCategoryIdParam) : null
     });
}, [categoryIdParam,subCategoryIdParam,subSubCategoryIdParam]);
    
    
    return (
        <div className ="home-container">
            <div className="food-container">
                <h2 id="food-container-h2"style={{position:"absolute",top:250,placeSelf:"center",color:"white",zIndex:1}}>Premiumlivsmedel, levererade färska</h2>
                <p style={{position:"absolute",top:300,placeSelf:"center",color:"white",zIndex:1}}>Upptäck utsökta köttdetaljer, hantverksbakat bröd och den allra färskaste havsmaten — direkt från noggrant utvalda kvalitetsleverantörer.</p>
                <div style={{position:"absolute",backgroundColor:"black",top:150,width:"100vw",height:400,zIndex:0,opacity:"55%" }}></div>
                <img src="IMG/food.jpg" alt="" className="food-pic" />
            </div>
            <ProductFilters
            filters={filters}
            updateFilter={updateFilter}>
            </ProductFilters>


            <div className ="product-grid">
                {products.map(p => (                   
                    <ProductCard key={p.id} product={p}></ProductCard>         
                ))}
            </div>
        </div>
    )

}