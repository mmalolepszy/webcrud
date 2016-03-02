using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCRUD.vNext.Infrastructure.ExtensionMethods
{
    /// <summary>
    /// Provides extension method that allows to perform common task of updating collection of objects in one layer based on collection of objects
    /// coming from upper layer - for example updating collection of entities based on collection of corresponding value objects.
    /// </summary>
    public static class CollectionMergeExtension
    {
        /// <summary>
        /// Returns merge handler which allows to define how collection of objects should be updated based on collection of other objects. 
        /// For example you can use this to update collection of entities based on collection of view models.
        /// Using fluent interface you have to specify:
        /// a method that will find matching entity and view model object,
        /// a method called when no corresponding entity is found, this method takes view model object and should convert it to entity, adding into collection is handled automatically,
        /// a method called when corresponding entity is found, it takes a pair - entity and matching view model - and should update entity based on view model data
        /// </summary>
        /// <typeparam name="TEntity">the entity type</typeparam>
        /// <typeparam name="TViewModel">the view model type</typeparam>
        /// <param name="target">target collection which will be updated (collection of entities)</param>
        /// <param name="src">source of data (view models)</param>
        /// <returns>metge handler</returns>
        public static MergeHandler<TEntity, TViewModel> MergeUsing<TEntity, TViewModel>(this ICollection<TEntity> target, IEnumerable<TViewModel> src)
        {
            return new MergeHandler<TEntity, TViewModel>(target, src);
        }
    }

    /// <summary>
    /// Class responsible for updating colllection of objects of one type with objects of other type - usually updating entities from value objects.
    /// </summary>
    /// <typeparam name="TEntity">the entity type</typeparam>
    /// <typeparam name="TViewModel">the view model type</typeparam>
    public class MergeHandler<TEntity, TViewModel> 
    {
        private ICollection<TEntity> entities;
	    private IEnumerable<TViewModel> viewModels;
	    private Func<TEntity,TViewModel,bool> matchingMethod;
	    private Func<TViewModel,TEntity> createNewItemMethod;
	    private Action<TEntity,TViewModel> updateItemMethod;
        private Action<TEntity> deleteItemMethod;

        /// <summary>
        /// Constructs merge handler instance
        /// </summary>
        /// <param name="entities">collection that we are going to update based on data in viewModels collection</param>
        /// <param name="viewModels">collection with data that we are going to use to update entities collection</param>
        public MergeHandler(ICollection<TEntity> entities, IEnumerable<TViewModel> viewModels)
        {
		    this.entities = entities;
		    this.viewModels = viewModels;
	    }

        /// <summary>
        /// Specifies a function that will be used to find matching elements in both collections
        /// </summary>
        /// <param name="matchingMethod"></param>
        public MergeHandler<TEntity, TViewModel> On(Func<TEntity, TViewModel, bool> matchingMethod)
        {
		    this.matchingMethod = matchingMethod;
		    return this;
	    }

        /// <summary>
        /// Specify a function that will be called when no matching element is found in entities collection.
        /// Use this function to create a new entity based on view model data. Adding to collection is handled automatically.
        /// </summary>
        public MergeHandler<TEntity, TViewModel> WhenNotMatchedCreate(Func<TViewModel, TEntity> createNewItemMethod)
        {
		    this.createNewItemMethod = createNewItemMethod;
		    return this;
	    }

        /// <summary>
        /// Specify a function that will be called when matching element is found in entities collection.
        /// Use this function to update existing entity based on view model data. 
        /// </summary>
        public MergeHandler<TEntity, TViewModel> WhenMatchedUpdate(Action<TEntity, TViewModel> updateItemMethod)
        {
		    this.updateItemMethod = updateItemMethod;
		    return this;
	    }

        public MergeHandler<TEntity, TViewModel> WhenRemoved(Action<TEntity> deleteItemMethod)
        {
            this.deleteItemMethod = deleteItemMethod;
            return this;
        }
	
        /// <summary>
        /// Executes merge algorithm
        /// </summary>
	    public void ExecuteMerge()
        {
		    var updatePairs = new Dictionary<TEntity,TViewModel>();
		    var toAdd = new List<TViewModel>();
		    //foreach element in viewModels try to find matching element in src
		    foreach (var vm in this.viewModels)
            {
			    var matchingEntity = this.entities.FirstOrDefault(entity=> this.matchingMethod(entity,vm));
			    if (matchingEntity == null)
                {
				    //On Add
				    toAdd.Add(vm);
			    }
                else 
                {
				    //On Update
				    updatePairs.Add(matchingEntity,vm);
			    }
		    }
		
		    //search for elements in source missing in incoming data (deleted elems)
		    var updatedEntites = updatePairs.Keys.ToList();
		    var deletedEntities = this.entities.Where(entity => !updatedEntites.Contains(entity)).ToList();		
		    foreach (var toDelEntity in deletedEntities)
            {
			    OnDelete(toDelEntity);
		    }
		    //update
		    foreach(var entityAndVmUpdatePair in updatePairs)
            {
			    OnUpdate(entityAndVmUpdatePair.Key, entityAndVmUpdatePair.Value);
		    }		
		    //add
		    foreach (var toAddItem in toAdd)
            {
			    OnAdd(toAddItem);
		    }
			
	    }
	
        /// <summary>
        /// Override this method if you want to customize item adding to entities collection.
        /// </summary>
        /// <param name="vm">view model item based on which new entity should be created and added to collection</param>
	    protected virtual void OnAdd(TViewModel vm)
        {
		    this.entities.Add(this.createNewItemMethod(vm));
	    }
	
        /// <summary>
        /// Override this method if you want to customize deletion of items from entities collection.
        /// </summary>
        /// <param name="entity">entity to delete</param>
	    protected virtual void OnDelete(TEntity entity)
        {
            if (this.deleteItemMethod == null)
            {
                this.entities.Remove(entity);
            }
            else
            {
                this.deleteItemMethod(entity);
            }

	    }
	
        /// <summary>
        /// Override this method if you want to customize the way entities are updated
        /// </summary>
        /// <param name="entity">entity to update</param>
        /// <param name="vm">matching view model data</param>
	    protected virtual void OnUpdate(TEntity entity,TViewModel vm)
        {
		    this.updateItemMethod(entity,vm);
	    }
    }
}