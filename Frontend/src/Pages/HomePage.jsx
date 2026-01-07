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

    //filter products by categoryid
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
            <div className="food-container">
                <h2 style={{position:"absolute",top:250,placeSelf:"center",color:"white",zIndex:1}}>Premiumlivsmedel, levererade färska</h2>
                <p style={{position:"absolute",top:300,placeSelf:"center",color:"white",zIndex:1}}>Upptäck utsökta köttdetaljer, hantverksbakat bröd och den allra färskaste havsmaten — direkt från noggrant utvalda kvalitetsleverantörer.</p>
                <div style={{position:"absolute",backgroundColor:"black",top:150,width:"100vw",height:400,zIndex:0,opacity:"55%" }}></div>
                <img src="IMG/food.jpg" alt="" className="food-pic" />
            </div>
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