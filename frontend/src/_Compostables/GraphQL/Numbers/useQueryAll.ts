import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IQueryResponse } from '../IQueryResponse';
import type { ApolloError } from '@apollo/client/errors';

const apolloClient = useApolloClient();
provideApolloClient(apolloClient)

const { result, onError, loading, onResult, error, refetch } = useQuery<IQueryResponse>(gql`
	query {
		numbers {
			id,
			all {
				id,
				isOwned,
				e164,
				canSend,
				isFallback,
			},
		},
	}
	`, null, {
	pollInterval: 1000*30,
});

onError((error: ApolloError) => {
	console.error('useQueryAll error', error);
});

const prop = computed(() => {
	if (!result.value) {
		return [];
	}
	return result.value?.numbers?.all || [];
});

const reFetch = () => {
	refetch();
}

const useQueryAll = () => {
	return {
		all: prop,
		allLoading: loading,
		allOnError: onError,
		allError: error,
		allOnResult: onResult,
		allRefetch: reFetch,
	}
}

export { useQueryAll }