
export default function ProductFilters({
    filters,
    updateFilter,
}){
    return (
        <div className ="filter-container">
            <h3>Filters</h3>
            
            <select onChange={(e) => updateFilter("brand", e.target.value)}>
                <option value="">Alla</option>
                <option value="Scan">Scan</option>
                <option value="ICA">ICA</option>
            </select>

            <select onChange={(e) => updateFilter("categoryId", e.target.value)}>
                <option value="">Alla</option>
                <option value="525">Kott</option>
            </select>

             <input  
                type="number"
                placeholder="Price"
                onChange={(e) => updateFilter(Number(e.target.value))}
            />
        </div>
    )
}