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
		cardDavContacts {
			id,
			all {
				id,
				addresses {
					type,
					label,
					addressParts,
				}
				birthday,
				eMails {
					type,
					eMail,
				},
				fullName,
				names,
				photoURI,
				prodId,
				revisionTime,
				telephoneNumbers {
					type,
					e164,
				},
				uid,
				version,
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
	return result.value?.cardDavContacts?.all || [];
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
		latestReFetch: reFetch,
	}
}

export { useQueryAll }